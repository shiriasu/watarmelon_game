using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FRUITS_TYPE
{
    さくらんぼ = 1,
    いちご,
    ぶどう,
    オレンジ,
    かき,
    りんご,
    なし,
    もも,
    パイナップル,
    メロン,
    すいか,
}

public class Fruits : MonoBehaviour
{
    public FRUITS_TYPE fruitsType;
    private static int fruits_serial = 0;
    private int my_serial;
    public bool isDestroyed = false;

    [SerializeField] private Fruits nextFruitsPrefab;

    private void Awake()
    {
        my_serial = fruits_serial;
        fruits_serial++;
    }

        private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Fruits otherFruits))
        {
            if (otherFruits.fruitsType == fruitsType)
            {
                Destroy(gameObject);
                
                if (nextFruitsPrefab != null && my_serial < otherFruits.my_serial)
                {
                    
                    Destroy(other.gameObject);
                    isDestroyed = true;
                    otherFruits.isDestroyed = true;

                    Vector3 center = (transform.position + other.transform.position) / 2;
                    Quaternion rotation = Quaternion.Lerp(transform.rotation, other.transform.rotation, 0.5f);
                    Fruits next = Instantiate(nextFruitsPrefab, center, rotation);

                    // ２つの速度の平均をとる
                    Rigidbody2D nextRb = next.GetComponent<Rigidbody2D>();
                    Vector3 velocity = (GetComponent<Rigidbody2D>().velocity + other.gameObject.GetComponent<Rigidbody2D>().velocity) / 2;
                    nextRb.velocity = velocity;

                    float angularVelocity = (GetComponent<Rigidbody2D>().angularVelocity + other.gameObject.GetComponent<Rigidbody2D>().angularVelocity) / 2;
                    nextRb.angularVelocity = angularVelocity;
                    

                }
            }
        }
    }
}