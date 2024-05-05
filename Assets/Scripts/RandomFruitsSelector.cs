using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFruitsSelector : MonoBehaviour
{
    [SerializeField] private Fruits[] fruitsPrefabs;

    private Fruits reservedFruits;
    public Fruits ReservedFruits
    {
        get { return reservedFruits; }
    }

    private void Start()
    {
        Pop();
    }

    public Fruits Pop()
    {
        Fruits ret = reservedFruits;

        int index = Random.Range(0, fruitsPrefabs.Length);
        reservedFruits = fruitsPrefabs[index];

        return ret;
    }
}
