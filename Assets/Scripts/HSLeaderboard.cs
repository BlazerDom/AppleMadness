using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class HSLeaderboard : MonoBehaviour
{
    private string hsLevel;
    public GameObject[] scoreArray;
    public static bool checkScore = false;
    //private bool doOnce = true;

    public void Awake()
    {
        //doOnce = true;
        Scene scene = SceneManager.GetActiveScene();
        var t = scene.name.Last();
        var i = int.Parse(t.ToString()) + 1;
        hsLevel = $"Level_{i}_HighScore";
    }

    // Update is called once per frame
    void Update()
    {
        //if (gameObject.activeInHierarchy)

            //HighScore.updateLBOnce = true;
    }

    public void ScoreUpdate()
    {
        for (int i = 0; i != scoreArray.Length; i++)
        {
            GameObject nameObject = scoreArray[i].transform.GetChild(1).gameObject;
            Text n = nameObject.GetComponent<Text>();
            GameObject scoreObject = scoreArray[i].transform.GetChild(2).gameObject;
            Text s = scoreObject.GetComponent<Text>();

            var playerNameID = PlayerPrefs.GetString("PlayerNameID");
            var b = playerNameID.Split(";".ToCharArray());
            var nameScore = PlayerPrefs.GetString(key: hsLevel + $"_{i}");
            var a = nameScore.Split(";".ToCharArray());
            var score = a.Where(x => x.StartsWith("Score:")).Select(a => a.Substring(a.IndexOf(":") + 1)).Single();
            var name = a.Where(x => x.StartsWith("Name:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single();
            var idBoard  = a.Where(x => x.StartsWith("ID:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single();
            var playerID = b.Where(x => x.StartsWith("ID:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single();
            n.text = name;
            s.text = score;

            if (idBoard == playerID)
            {
                n.color = Color.green;
                s.color = Color.green;
            }
        }

    }


}
