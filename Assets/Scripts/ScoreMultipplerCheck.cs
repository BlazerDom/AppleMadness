using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMultipplerCheck : MonoBehaviour
{
    private ApplePicker apPicker;
    private Text multy;
    private void Start()
    {

        multy = gameObject.GetComponent<Text>();
    }
    void FixedUpdate()
    {
        ApplePicker apPicker = Camera.main.GetComponent<ApplePicker>();
        if (apPicker != null) multy.text = $"x{apPicker.scoreMultipler}";
    }
}
