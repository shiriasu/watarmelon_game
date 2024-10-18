using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class renamestart : MonoBehaviour
{
    public void SwitchScene()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}