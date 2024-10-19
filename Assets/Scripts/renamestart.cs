using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class renamestart : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Enterキーが押されたらシーンを切り替える
        if (Input.GetKeyDown(KeyCode.Return)) // KeyCode.ReturnはEnterキーを指します
        {
            SwitchScene();
        }
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
