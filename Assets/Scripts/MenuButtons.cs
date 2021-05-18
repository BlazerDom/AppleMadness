using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void RestartPressed()
    {
        SceneManager.LoadScene("_Scene_ApplePicker_0");
    }

    public void ExitPressed()
    {
        Application.Quit();
    }
}
