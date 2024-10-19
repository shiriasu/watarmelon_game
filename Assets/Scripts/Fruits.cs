using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum FRUITS_TYPE
{
    さくらんぼ = 0,
    いちご,
    ぶどう,
    オレンジ,
    かき,
    りんご,
    なし,
    もも,
    パイナップル,
}

public class Fruits : MonoBehaviour
{
    public FRUITS_TYPE fruitsType;
    private static int fruits_serial = 0;
    private int my_serial;
    public bool isDestroyed = false;
    public static UnityEvent OnGameOver = new UnityEvent();
    private bool isInside = false;

    [SerializeField] private Fruits nextFruitsPrefab;
    [SerializeField] private int score;

    // Instantiateされたフルーツのカウントを追跡するための変数
    private static int instantiateCount = 0;

    // 5回目で生成するためのフルーツPrefab
    [SerializeField] private Fruits rewardFruitsPrefab;

    // FruitsDropperオブジェクトをプレハブとして持っているため、シーン上のインスタンスを取得
    private Rigidbody2D fruitsDropper;


    public static UnityEvent<int> OnScoreAdded = new UnityEvent<int>();

    private void Awake()
    {
        my_serial = fruits_serial;
        fruits_serial++;

        // シーン上のFruitsDropperオブジェクトを探す
        fruitsDropper = FindObjectOfType<Rigidbody2D>();
    }

    IEnumerator Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        while (rb.isKinematic)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        if (!isInside)
        {
            OnGameOver.Invoke();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        isInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isDestroyed)
        {
            return;
        }
        OnGameOver.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isDestroyed)
        {
            return;
        }

        if (other.gameObject.TryGetComponent(out Fruits otherFruits))
        {
            if (otherFruits.fruitsType == fruitsType)
            {
                if (my_serial < otherFruits.my_serial)
                {
                    OnScoreAdded.Invoke(score);

                    isDestroyed = true;
                    otherFruits.isDestroyed = true;
                    Destroy(gameObject);
                    Destroy(other.gameObject);

                    if (nextFruitsPrefab == null)
                    {
                        return;
                    }

                    Vector3 center = (transform.position + other.transform.position) / 2;
                    Quaternion rotation = Quaternion.Lerp(transform.rotation, other.transform.rotation, 0.5f);
                    Fruits next = Instantiate(nextFruitsPrefab, center, rotation);

                    // ２つの速度の平均をとる
                    Rigidbody2D nextRb = next.GetComponent<Rigidbody2D>();
                    Vector3 velocity = (GetComponent<Rigidbody2D>().velocity + other.gameObject.GetComponent<Rigidbody2D>().velocity) / 2;
                    nextRb.velocity = velocity;

                    float angularVelocity = (GetComponent<Rigidbody2D>().angularVelocity + other.gameObject.GetComponent<Rigidbody2D>().angularVelocity) / 2;
                    nextRb.angularVelocity = angularVelocity;

                    // Instantiateのカウントをインクリメント
                    instantiateCount++;

                    if (instantiateCount >= 5)
                    {
                        instantiateCount = 0; // カウントをリセット

                        if (rewardFruitsPrefab != null)
                        {
                            // シーン上のFruitsDropperの位置を確認し、リワードフルーツを生成
                            if (fruitsDropper != null)
                            {
                                Vector3 spawnPosition = fruitsDropper.transform.position + new Vector3(0, 3, 0); // オフセットを加える
                                Fruits reward = Instantiate(rewardFruitsPrefab, spawnPosition, Quaternion.identity);

                                if (nextFruitsPrefab == null)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
