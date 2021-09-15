using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class HSLeaderboard : MonoBehaviour
{
    private string hsLevel;
    public int number = 0;
    private bool doOnce = true;
    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        var t = scene.name.Last();
        var i = int.Parse(t.ToString()) + 1;
        hsLevel = $"Level_{i}_HighScore";
    }
    public void Awake()
    {
        doOnce = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            ScoreUpdate(number - 1);
            HighScore.updateLBOnce = true;
        }
        
    }
    public void ScoreUpdate(int i)
    {
        Text t = gameObject.GetComponent<Text>();
        var nameScore = PlayerPrefs.GetString(key: hsLevel + $"_{i}");
        var a = nameScore.Split(";".ToCharArray());
        var score = a.Where(x => x.StartsWith("Score:")).Select(a => a.Substring(a.IndexOf(":") + 1)).Single();
        var name = a.Where(x => x.StartsWith("Name:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single();
        t.text = number.ToString() + ". " + name + "    " + score;
    }


}
