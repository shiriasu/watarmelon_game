using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitsDropper : MonoBehaviour
{
    [SerializeField] private RandomFruitsSelector randomFruitsSelector;
    public float moveSpeed = 5f;

    [SerializeField] private float coolTime = 1f;
    private Fruits fruitsInstance;

    private void Start()
    {
        StartCoroutine(HandleFruits(coolTime));
        Fruits.OnGameOver.AddListener(() => enabled = false);
    }

    private IEnumerator HandleFruits(float delay)
    {
        yield return new WaitForSeconds(delay);
        var fruitsPrefab = randomFruitsSelector.Pop();
        fruitsInstance = Instantiate(fruitsPrefab, transform.position, Quaternion.identity);
        fruitsInstance.transform.SetParent(transform);
        fruitsInstance.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && fruitsInstance != null)
        {
            fruitsInstance.GetComponent<Rigidbody2D>().isKinematic = false;
            fruitsInstance.transform.SetParent(null);
            fruitsInstance = null;
            StartCoroutine(HandleFruits(coolTime));
        }

        float horizontal = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        float x = Mathf.Clamp(transform.position.x + horizontal, -2.5f, 2.5f);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}