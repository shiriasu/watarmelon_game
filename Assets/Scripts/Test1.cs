using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private RandomFruitsSelector randomFruitsSelector;
    public float moveSpeed = 5f;

    [SerializeField] private float coolTime = 1f;
    private Fruits fruitsInstance;

    [SerializeField] private string _moveActionName = "Move";
    [SerializeField] private string _fireActionName = "Fire";

    private void Start()
    {
        //_playerInput.actions[_moveActionName].started   += OnMove;
        _playerInput.actions[_moveActionName].performed += OnMove;
        //_playerInput.actions[_moveActionName].canceled  += OnMove;
        _playerInput.actions[_fireActionName].performed += OnFire;

        StartCoroutine(HandleFruits(coolTime));
        Fruits.OnGameOver.AddListener(() => enabled = false);
    }

    private void OnDestroy()
    {
        _playerInput.actions[_moveActionName].performed -= OnMove;
        _playerInput.actions[_fireActionName].performed -= OnFire;
    }

    private IEnumerator HandleFruits(float delay)
    {
        yield return new WaitForSeconds(delay);
        var fruitsPrefab = randomFruitsSelector.Pop();
        fruitsInstance = Instantiate(fruitsPrefab, transform.position, Quaternion.identity);
        fruitsInstance.transform.SetParent(transform);
        fruitsInstance.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        float horizontal = context.ReadValue<float>() * moveSpeed * Time.deltaTime;
        float x = Mathf.Clamp(transform.position.x + horizontal, -2.5f, 2.5f);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (fruitsInstance != null)
        {
            fruitsInstance.GetComponent<Rigidbody2D>().isKinematic = false;
            fruitsInstance.transform.SetParent(null);
            fruitsInstance = null;
            StartCoroutine(HandleFruits(coolTime));
        }  
    }
}
