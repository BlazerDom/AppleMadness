using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public List<int> LevelLeaders;
    public List<ScoreInfo> LevelLeaders2;
    public List<string> LevelLeadersName;
    public static RectTransform ScorePanelRectT;
    public string hsLevel = "Level_1_HighScore";
    public string textLevel = "Level 1";

    private int secondScore = 0;
    private int index = 10;
    private void Awake()
    {
        //LevelLeadersName = new List<string>();
        //LevelLeaders = new List<int>();
        //LevelLeaders2 = new List<ScoreInfo>();

        //var s = "Id:1;Name:Ant;Score:1000;Level:1";
        //var a = s.Split(";".ToCharArray());
        //var si = new ScoreInfo
        //{
        //    ID = int.Parse(a.Where(x => x.StartsWith("ID:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single()),
        //    Name = a.Where(x => x.StartsWith("Name:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single(),
        //    Score = int.Parse(a.Where(x => x.StartsWith("Score:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single()),
        //    Level = int.Parse(a.Where(x => x.StartsWith("Level:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single()),
        //};

        //var sArr = new[] { "ID:1;Name:Ant;Score:1000;Level:1", "ID:2;Name:Ant;Score:2000;Level:1" };
        //var aArr = sArr.Select(x => x.Split(";".ToCharArray()));
        //var siArr = aArr.Select(x => new ScoreInfo
        //{
        //    ID = int.Parse(a.Where(x => x.StartsWith("ID:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single()),
        //    Name = a.Where(x => x.StartsWith("Name:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single(),
        //    Score = int.Parse(a.Where(x => x.StartsWith("Score:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single()),
        //    Level = int.Parse(a.Where(x => x.StartsWith("Level:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single()),
        //}).ToArray();

        //var sConcrete = siArr.SingleOrDefault(x => x.ID == 1);
        //if (sConcrete != null)
        //{
        //    sConcrete.Score = 5000;

        //    var sToSave = string.Join(";", new string[] 
        //    {
        //        "ID:" + sConcrete.ID,
        //        "Name:" + sConcrete.Name,
        //        "Score:" + sConcrete.Score,
        //        "Level:" + sConcrete.Level 
        //    }); 
        //}
        



        //var view = siArr.OrderBy(x => x.Score).Select(x => new { x.Name, x.Score });



        CreateLeadersList();

        if (PlayerPrefs.HasKey(key: hsLevel + $"_{0}"))
        {
            score = PlayerPrefs.GetInt(key: hsLevel + $"_{0}");
        }
        Invoke("UpdateLeadersList", 3f);
    }

    static public int score = 1000;

    private void Update()
    {
        Text gt = this.GetComponent<Text>();
        gt.text = textLevel + $" High Score: {score}";

        Text gt = this.GetComponent<Text>();
        gt.text = textLevel + $" High Score: {score}";

        if (score > PlayerPrefs.GetInt(key: hsLevel))
        {
            PlayerPrefs.SetInt(key: hsLevel, score);
        }
    }

    public void SetTransformPos(float x, float y, float ax, float ay)
    {
        ScorePanelRectT.anchorMin = new Vector2(ax, ay);
        ScorePanelRectT.anchorMax = new Vector2(ax, ay);
        ScorePanelRectT.position = new Vector2(x, y);    
    }

    public void CreateLeadersList()
    {
        for (int i = 0; i < 11; i++)
        {

            if (PlayerPrefs.HasKey(key: hsLevel + $"_{i}"))
            {
                LevelLeaders.Add(PlayerPrefs.GetInt(key: hsLevel + $"_{i}"));
                PlayerPrefs.SetInt(key: hsLevel + $"_{i}", LevelLeaders[i]);
            }
            if (!PlayerPrefs.HasKey(key: hsLevel + $"_{i}"))
            {
                LevelLeaders.Add(1000);
                PlayerPrefs.SetInt(key: hsLevel + $"_{i}", 1000);
            }
            if (i == 10)
            {
                LevelLeaders[i] = 1000;
                PlayerPrefs.SetInt(key: hsLevel + $"_{i}", LevelLeaders[i]);
                LevelLeaders = LevelLeaders.OrderByDescending(x => x).ToList();
            }
        }
    }

    public void UpdateLeadersList()
    {
        secondScore = int.Parse(Basket.scoreGT.text);
        if (secondScore > LevelLeaders[index])
        {
            LevelLeaders[index] = secondScore;
            PlayerPrefs.SetInt(key: hsLevel + $"_{index}", LevelLeaders[index]);
            if(index != 0)
            {
                LevelLeaders = LevelLeaders.OrderByDescending(x => x).ToList();
                index = LevelLeaders.IndexOf(secondScore);
            }
        }
        Invoke("UpdateLeadersList", 3f);
    }

    public void SortLeaderBoards()
    {
        LevelLeaders = LevelLeaders.OrderByDescending(x => x).ToList();
        index = LevelLeaders.IndexOf(secondScore);
    }
}

public class ScoreInfo
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }
    public int Level { get; set; }
}
