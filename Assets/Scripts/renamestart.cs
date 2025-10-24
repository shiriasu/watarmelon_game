using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // 1. Input System を使うために追加

public class renamestart : MonoBehaviour
{
    void Update()
    {
        // 2. 現在接続されているゲームパッドを取得
        var gamepad = Gamepad.current;
        
        // 3. キーボードのEnterキーが押されたかチェック
        bool keyboardStart = Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame;

        // 4. ゲームパッドのボタンが押されたかチェック
        bool gamepadStart = false;
        if (gamepad != null)
        {
            // buttonSouth = Aボタン
            // startButton = スタート/Optionsボタン
            if (gamepad.buttonEast.wasPressedThisFrame || gamepad.startButton.wasPressedThisFrame)
            {
                gamepadStart = true;
            }
        }

        // 5. どちらかで押されていたらシーン切り替え
        if (keyboardStart || gamepadStart)
        {
            SwitchScene();
        }
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}