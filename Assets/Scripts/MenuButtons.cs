using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    private bool musicToggle = true;
    public void RestartPressed()
    {
        SceneManager.LoadScene("_Scene_ApplePicker_0");
    }

    public void ExitPressed()
    {
        Application.Quit();
    }

    public void MusicToggle()
    {
        
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        musicToggle = !musicToggle;
        audioSource.enabled = musicToggle;
    }

    public void ResetScore()
    {
        HighScore.score = 1000;
        PlayerPrefs.SetInt("HighScore", 1000);
    }
}
