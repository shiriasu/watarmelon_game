using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    public GameObject gameOverPanel;

    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private RandomFruitsSelector randomFruitsSelector;
    public float moveSpeed = 5f;

    [SerializeField] private float coolTime = 1f;
    private Fruits fruitsInstance;

    [SerializeField] private string _moveActionName = "Move";
    [SerializeField] private string _fireActionName = "Fire";

    [SerializeField] Test otherFruitsDropper;

    // 現在の移動方向を保存するための変数
    private Vector2 moveInput = Vector2.zero;

    private void Start()
    {
        if (_playerInput == null)
        {
            Debug.LogError("PlayerInput is not assigned!");
            return;
        }

        _playerInput.actions[_moveActionName].performed += OnMove;
        _playerInput.actions[_moveActionName].canceled += OnMoveCancel;
        _playerInput.actions[_fireActionName].performed += OnFire;

        StartCoroutine(HandleFruits(coolTime));
        Fruits.OnGameOver.AddListener(OnGameOver);

        // PlayerInput のインスタンスが正しいか確認する
        Debug.Log("Player Index: " + _playerInput.playerIndex);
        Debug.Log("Control Scheme: " + _playerInput.currentControlScheme);
    }

    private void OnDestroy()
    {
        if (_playerInput != null)
        {
            _playerInput.actions[_moveActionName].performed -= OnMove;
            _playerInput.actions[_moveActionName].canceled -= OnMoveCancel;
            _playerInput.actions[_fireActionName].performed -= OnFire;
        }
    }

    private IEnumerator HandleFruits(float delay)
    {
        yield return new WaitForSeconds(delay);
        var fruitsPrefab = randomFruitsSelector.Pop();
        fruitsInstance = Instantiate(fruitsPrefab, transform.position, Quaternion.identity);
        fruitsInstance.transform.SetParent(transform);
        fruitsInstance.GetComponent<Fruits>().otherFruitsDropper = otherFruitsDropper;
        fruitsInstance.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    // キーを押したとき
    private void OnMove(InputAction.CallbackContext context)
    {
        if (gameOverPanel != null && gameOverPanel.activeSelf) return; // ゲームオーバーパネルが表示されている場合、入力無効化

        moveInput = context.ReadValue<Vector2>(); // 移動入力を取得
    }

    // キーを離したとき
    private void OnMoveCancel(InputAction.CallbackContext context)
    {
        if (gameOverPanel != null && gameOverPanel.activeSelf) return; // ゲームオーバーパネルが表示されている場合、入力無効化

        moveInput = Vector2.zero; // 移動入力をリセット
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (_playerInput == null || fruitsInstance == null || (gameOverPanel != null && gameOverPanel.activeSelf)) return;

        if (fruitsInstance != null)
        {
            fruitsInstance.GetComponent<Rigidbody2D>().isKinematic = false;
            fruitsInstance.transform.SetParent(null);
            fruitsInstance = null;
            StartCoroutine(HandleFruits(coolTime));
        }
    }

    private void OnGameOver()
    {
        // ゲームオーバーパネルが表示されたときに入力を無効化する
        if (_playerInput != null)
        {
            _playerInput.enabled = false;
        }
        else
        {
            Debug.LogWarning("PlayerInput is null in OnGameOver!");
        }
    }

    private void Update()
    {
        if (moveInput != Vector2.zero && (gameOverPanel == null || !gameOverPanel.activeSelf))
        {
            // フレームごとに移動量を計算
            float horizontal = moveInput.x * moveSpeed * Time.deltaTime;

            // 移動範囲を制限
            float x = Mathf.Clamp(transform.position.x + horizontal, -2.5f, 2.5f);

            // 新しい位置に移動
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
    }
}
