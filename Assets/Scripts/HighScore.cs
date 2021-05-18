using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public static RectTransform ScorePanelRectT;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            score = PlayerPrefs.GetInt("HighScore");
        }

        PlayerPrefs.SetInt("HighScore", score);
    }

    static public int score = 1000;

    private void Update()
    {
        Text gt = this.GetComponent<Text>();
        gt.text = $"High Score: {score}";

        if(score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public void SetTransformPos(float x, float y, float ax, float ay)
    {
        ScorePanelRectT.anchorMin = new Vector2(ax, ay);
        ScorePanelRectT.anchorMax = new Vector2(ax, ay);
        ScorePanelRectT.position = new Vector2(x, y);    
    }
}
