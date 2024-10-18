using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class InputActionAssets : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveAction;

    [SerializeField] private RandomFruitsSelector randomFruitsSelector;
    public float moveSpeed = 5f;

    [SerializeField] private float coolTime = 1f;
    private Fruits fruitsInstance;

    private void Awake()
    {
        _moveAction.action.performed += OnMove;

        _moveAction.action.performed += OnMove;
    }

    private void OnDestroy()
    {
        _moveAction.action.performed -= OnMove;
        _moveAction.action.performed -= OnMove;
    }

    private void OnEnable() => _moveAction.action.Enable();
    private void OnDisable() => _moveAction.action.Disable();

    private void OnMove(InputAction.CallbackContext context)
    {
        float horizontal = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
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

    private IEnumerator HandleFruits(float delay)
    {
        yield return new WaitForSeconds(delay);
        var fruitsPrefab = randomFruitsSelector.Pop();
        fruitsInstance = Instantiate(fruitsPrefab, transform.position, Quaternion.identity);
        fruitsInstance.transform.SetParent(transform);
        fruitsInstance.GetComponent<Rigidbody2D>().isKinematic = true;
    }
}