using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayNextFruits : MonoBehaviour
{
    [SerializeField] private Image nextFruitsImage;
    [SerializeField] private RandomFruitsSelector randomFruitsSelector;

    private void Update()
    {
        if (randomFruitsSelector.ReservedFruits != null)
        {
            nextFruitsImage.sprite = randomFruitsSelector.ReservedFruits.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
