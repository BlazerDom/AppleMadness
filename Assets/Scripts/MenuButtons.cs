using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    private bool musicToggle = true;
    public GameObject menu;
    public GameObject levelsMenu;
    public string hsLevel = "Level_1_HighScore";

    public void StartButton()
    {
        ApplePicker ap = Camera.main.GetComponent<ApplePicker>();
        ap.MenuOnOff();
    }
    public void RestartPressed()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
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
        PlayerPrefs.SetInt(key: hsLevel, 1000);
    }

    public void ChangeLevelButton()
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
            levelsMenu.SetActive(true);
        }
    }

    public void BackToMenu()
    {
        if (levelsMenu.activeSelf)
        {
            levelsMenu.SetActive(false);
            menu.SetActive(true);
        }
    }

    public void LoadLevelPressed(int i)
    {
        SceneManager.LoadScene($"_Scene_ApplePicker_{i -= 1}");
    }
}
