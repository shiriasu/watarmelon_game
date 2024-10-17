using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionAssets : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveAction;

    [SerializeField] private RandomFruitsSelector randomFruitsSelector;
    public float moveSpeed = 5f;

    //[SerializeField] private float coolTime = 1f;
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
}