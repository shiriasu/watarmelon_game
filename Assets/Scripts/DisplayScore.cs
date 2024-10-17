using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScore : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void Start()
    {
        Fruits.OnGameOver.AddListener(() => Debug.Log("Game Over"));

        gameOverPanel.SetActive(false);
        Fruits.OnGameOver.AddListener(() => gameOverPanel.SetActive(true));
    }
}