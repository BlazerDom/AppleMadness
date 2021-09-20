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
            Text t = scoreArray[i].GetComponent<Text>();
            var nameScore = PlayerPrefs.GetString(key: hsLevel + $"_{i}");
            var a = nameScore.Split(";".ToCharArray());
            var score = a.Where(x => x.StartsWith("Score:")).Select(a => a.Substring(a.IndexOf(":") + 1)).Single();
            var name = a.Where(x => x.StartsWith("Name:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single();
            var j = i + 1;
            t.text = j.ToString() + ". " + name + "    " + score;
        }

    }


}
