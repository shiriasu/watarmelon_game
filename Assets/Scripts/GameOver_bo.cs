using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver_bo : MonoBehaviour
{
    public void SceneChange()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
