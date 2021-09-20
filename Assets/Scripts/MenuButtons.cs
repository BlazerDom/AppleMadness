using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MenuButtons : MonoBehaviour
{
    private bool musicToggle = true;
    public GameObject menu;
    public GameObject levelsMenu;
    public GameObject leaderboard;
    public InputField inputPlayerName;
    public GameObject highScore;

    private HighScore hsRef;
    private HSLeaderboard lbRef;

    public string hsLevel = "Level_1_HighScore";
    [SerializeField] private string id = Guid.NewGuid().ToString();

    private void Start()
    {
        hsRef = highScore.GetComponent<HighScore>();
        lbRef = leaderboard.GetComponent<HSLeaderboard>();

        var a = PlayerPrefs.GetString("PlayerNameID").Split(";".ToCharArray());
        var name = a.Where(x => x.StartsWith("Name:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single();

        inputPlayerName.text = name;
    }
    public void StartButton()
    {
        ApplePicker ap = Camera.main.GetComponent<ApplePicker>();
        ap.gameOver = false;
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
        {
            hsRef.levelLeaders[i] = "ID:0;Name:Player;Score:1000;";
            PlayerPrefs.SetString(key: hsLevel + $"_{i}", "ID:0;Name:Player;Score:1000;");
        }
        hsRef.CreateLeadersList(0);
        lbRef.ScoreUpdate();
        hsRef.UpdateLeadersList();
    }

    public void ChangeLevelButton()
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
            levelsMenu.SetActive(true);
        }
    }

    public void LeaderboardButton()
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
            leaderboard.SetActive(true);
        }
        if (leaderboard.activeSelf)
        {
            //HSLeaderboard lboardScriptRef = leaderboard.GetComponent<HSLeaderboard>();
            hsRef.UpdateLeadersList();
            lbRef.ScoreUpdate();
        }
    }


    public void BackToMenu()
    {
        if (!menu.activeSelf)
        {
            leaderboard.SetActive(false);
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

        //inputPlayerName = GetComponent<InputField>();
        id = Guid.NewGuid().ToString();
        PlayerPrefs.SetString("PlayerNameID", $"ID:{id};Name:{inputPlayerName.text}");
        hsRef.CreateLeadersList(10);
        hsRef.UpdateLeadersList();
        lbRef.ScoreUpdate();
    }
}
