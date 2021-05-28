using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        for(int i = 0; i < 10; i++)
        PlayerPrefs.SetInt(key: hsLevel + $"_{i}", 1000);
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
    
    public void EnterName()
    {
        InputField ifCharName = gameObject.GetComponent<InputField>();
        PlayerPrefs.SetString("PlayerName", ifCharName.text);
    }
}
