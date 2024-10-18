using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DisplayScore : MonoBehaviour
{
    private float step_time; //経過時間カウント用
    public GameObject gameOverPanel;

    private void Start()
    {
        step_time = 0.0f;       // 経過時間初期化
        Fruits.OnGameOver.AddListener(() => Debug.Log("Game Over"));

        gameOverPanel.SetActive(false);
        Fruits.OnGameOver.AddListener(() => gameOverPanel.SetActive(true));
    }

    // Update is called once per frame
    private void Update()
    {
        // 経過時間をカウント
        if (gameOverPanel.activeSelf) // gameOverPanelがアクティブかどうかを確認
        {
            step_time += Time.deltaTime;
            // 10秒後に画面遷移（HomeSceneへ移動）
            if (step_time >= 10.0f)
            {
                SceneManager.LoadScene("HomeScene");
            }
        }
    }
}
