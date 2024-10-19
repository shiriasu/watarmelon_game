using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver_bo : MonoBehaviour
{
    void Update()
    {
        // Enterキーが押されたらシーンを切り替える
        if (Input.GetKeyDown(KeyCode.Return)) // KeyCode.ReturnはEnterキーを指します
        {
            SceneChange();
        }
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
