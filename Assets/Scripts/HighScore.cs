using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public static RectTransform ScorePanelRectT;
    public string hsLevel = "LevelOneHighScore";
    public string textLevel = $"Level 1 High Score: {score}";
    private void Awake()
    {
        if (PlayerPrefs.HasKey(key: hsLevel))
        {
            score = PlayerPrefs.GetInt(key: hsLevel);
        }

        PlayerPrefs.SetInt(key: hsLevel, score);
    }

    static public int score = 1000;

    private void Update()
    {
        Text gt = this.GetComponent<Text>();
        gt.text = textLevel;

        if(score > PlayerPrefs.GetInt(key: hsLevel))
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
}
