using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    public GameObject gameOverPanel;
    private bool isGameOver = false;

    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private RandomFruitsSelector randomFruitsSelector;
    public float moveSpeed = 5f;

    [SerializeField] private float coolTime = 1f;
    private Fruits fruitsInstance;

    [SerializeField] private string _moveActionName = "Move";
    [SerializeField] private string _fireActionName = "Fire";

    [SerializeField] Test otherFruitsDropper;

    private float autoDropTimer = 0.0f;
    private const float AUTO_DROP_TIME_LIMIT = 10.0f; // 10秒に設定

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
        //Fruits.OnGameOver.AddListener(OnGameOver);

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

        Fruits fruitsComponent = fruitsInstance.GetComponent<Fruits>();
        fruitsComponent.owner = this; // owner を設定

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

    private void DropFruit()
    {
        // フルーツを持っていないか、ゲームオーバーなら何もしない
        if (fruitsInstance == null || (gameOverPanel != null && gameOverPanel.activeSelf))
        {
            return;
        }

        // フルーツを落とす処理
        fruitsInstance.GetComponent<Rigidbody2D>().isKinematic = false;
        fruitsInstance.transform.SetParent(null);
        fruitsInstance = null;
        StartCoroutine(HandleFruits(coolTime));
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (_playerInput == null || fruitsInstance == null || (gameOverPanel != null && gameOverPanel.activeSelf)) return;

        DropFruit();
        autoDropTimer = 0.0f; // 手動で落とした場合もタイマーリセット
    }

    public void TriggerGameOver()
    {
        if (isGameOver)
        {
            return;
        }
        
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 入力を無効化する
        if (_playerInput != null)
        {
            _playerInput.enabled = false;
        }
    }

    private void Update()
    {
        if (gameOverPanel != null && gameOverPanel.activeSelf)
        {
            return; // ゲームオーバー中は以降の処理をしない
        }

        autoDropTimer += Time.deltaTime; // 毎フレーム時間を加算

        // 10秒経過したら
        if (autoDropTimer >= AUTO_DROP_TIME_LIMIT)
        {
            DropFruit();          // フルーツを落とす
            autoDropTimer = 0.0f; // タイマーをリセット
        }

        if (moveInput != Vector2.zero)
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
