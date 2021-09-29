using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
  using System.Runtime.Serialization.Formatters.Binary;

public class HighScore : MonoBehaviour
{
    public List<string> levelLeaders;
    public List<ScoreInfo> LevelLeaders2;
    public List<string> LevelLeadersName;
    public static RectTransform ScorePanelRectT;
    public string hsLevel = "Level_1_HighScore";
    public string textLevel = "Level 1";
    public string playerNameID = "ID:0;Name:Player";
    private string playerNameIDBase = "ID:0;Name:Player";

    public static bool updateLBOnce = true;
    private int secondScore = 0;
    private int index = 10;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("PlayerNameID"))
        {
            playerNameID = PlayerPrefs.GetString("PlayerNameID");
            var id = Guid.NewGuid().ToString();
            var a = playerNameID.Split(";".ToCharArray());
            var n = a.Where(x => x.StartsWith("Name:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single();
            PlayerPrefs.SetString("PlayerNameID", $"ID:{id};Name:{n}");
            playerNameID = PlayerPrefs.GetString("PlayerNameID");
        }
        levelLeaders = new List<string>();

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



        CreateLeadersList(0);

        if (PlayerPrefs.HasKey(key: hsLevel + $"_{0}"))
        {
            var s = PlayerPrefs.GetString(key: hsLevel + $"_{0}");
            var a = s.Split(";".ToCharArray());
            score = int.Parse(a.Where(x => x.StartsWith("Score:")).Select(a => a.Substring(a.IndexOf(":") + 1)).Single());
        }
    }

    static public int score = 1000;

    private void Update()
    {
        Text gt = this.GetComponent<Text>();
        gt.text = textLevel + $" High Score: {score}";

        if (updateLBOnce)
        {
            UpdateLeadersList();
        }

        //Text gt = this.GetComponent<Text>();
        //gt.text = textLevel + $" High Score: {score}";

        //if (score > PlayerPrefs.GetInt(key: hsLevel))
        //{
        //    PlayerPrefs.SetInt(key: hsLevel, score);
        //}
    }

    public void SetTransformPos(float x, float y, float ax, float ay)
    {
        ScorePanelRectT.anchorMin = new Vector2(ax, ay);
        ScorePanelRectT.anchorMax = new Vector2(ax, ay);
        ScorePanelRectT.position = new Vector2(x, y);    
    }

    public void CreateLeadersList(int iLoops)
    {
        playerNameID = PlayerPrefs.GetString("PlayerNameID");

        for (int i = iLoops;  i < 11; i++)
        {

            if (PlayerPrefs.HasKey(key: hsLevel + $"_{i}"))
            {
                if(PlayerPrefs.GetString(key: hsLevel + $"_{i}") == "")
                {
                    levelLeaders.Add(playerNameIDBase + $";Score:1000");
                    PlayerPrefs.SetString(key: hsLevel + $"_{i}", levelLeaders[i]);
                }
                levelLeaders.Add(PlayerPrefs.GetString(key: hsLevel + $"_{i}"));
                PlayerPrefs.SetString(key: hsLevel + $"_{i}", levelLeaders[i]);
            }
            if (!PlayerPrefs.HasKey(key: hsLevel + $"_{i}"))
            {
                levelLeaders.Add(playerNameIDBase + $";Score:1000");
                PlayerPrefs.SetString(key: hsLevel + $"_{i}", levelLeaders[i]);
            }
            if (i == 10)
            {
                for (int j = 11; j < levelLeaders.Count;)
                {
                    levelLeaders.RemoveAt(levelLeaders.Count - 1);
                }              
                levelLeaders[i] = playerNameID + $";Score:1000";
                PlayerPrefs.SetString(key: hsLevel + $"_{i}", levelLeaders[i]);
                //levelLeaders = levelLeaders.OrderByDescending(x => x.Split(";".ToCharArray()).Where(a => a.StartsWith("Score:")).Select(a => a.Substring(a.IndexOf(":") + 1)).Single()).ToList();
            }
        }
    }

    public void UpdateLeadersList()
    {
        if (PlayerPrefs.HasKey("PlayerNameID")) playerNameID = PlayerPrefs.GetString("PlayerNameID");
        secondScore = int.Parse(Basket.scoreGT.text);
        var a = levelLeaders[index].Split(";".ToCharArray());
        var b = playerNameID.Split(";".ToCharArray());
        var score = int.Parse(a.Where(x => x.StartsWith("Score:")).Select(a => a.Substring(a.IndexOf(":") + 1)).Single());
        var id = b.Where(x => x.StartsWith("ID:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single();
        var name = b.Where(x => x.StartsWith("Name:")).Select(x => x.Substring(x.IndexOf(":") + 1)).Single();

        //if (secondScore > score)
        //{
            levelLeaders[index] = string.Join(";", new string[]
            {
                "ID:" + id,
                "Name:" + name,
                "Score:" + secondScore,
            });

            var llForIndex = levelLeaders[index];
            PlayerPrefs.SetString(key: hsLevel + $"_{index}", levelLeaders[index]);

        //var b = levelLeaders.Select(x => int.Parse(x.Split(";".ToCharArray()).Where(a => a.StartsWith("Score:")).Select(a => a.Substring(a.IndexOf(":") + 1)).Single())).ToArray();
            levelLeaders = levelLeaders.OrderByDescending(x => int.Parse(x.Split(";".ToCharArray()).Where(a => a.StartsWith("Score:")).Select(a => a.Substring(a.IndexOf(":") + 1)).Single())).ToList();
            index = levelLeaders.IndexOf(llForIndex);

            for (int i = 0; i < 11; i++)
            {
                PlayerPrefs.SetString(key: hsLevel + $"_{i}", levelLeaders[i]);
            }
        //}
        updateLBOnce = false;
    }

    //public void SortLeaderBoards()
    //{
    //    LevelLeaders = LevelLeaders.OrderByDescending(x => x).ToList();
    //    index = LevelLeaders.IndexOf(secondScore);
    //}
}

public class ScoreInfo
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }
    public int Level { get; set; }
}
