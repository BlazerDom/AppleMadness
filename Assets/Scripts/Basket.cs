using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{
    public int score = 0;

    public float speed = 10f;

    public float alpha = 1;

    public int basketHP = 10;

    public Text scoreGT;

    public Vector3 pos = Vector3.zero;

    private void Start()
    {
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        scoreGT = scoreGO.GetComponent<Text>();
    }
    void Update()
    {
        Vector3 touchPos2D = Vector3.zero;
        Vector3 touchPos3D = Vector3.zero;

        Vector3 mousePos2D = Vector3.zero;
        Vector3 mousePos3D = Vector3.zero;

        Vector3 inputPos = Vector3.zero;


        if (Input.touchCount > 0)
        {
            touchPos2D = Input.GetTouch(0).position;
            touchPos2D.z = -Camera.main.transform.position.z;
            touchPos3D = Camera.main.ScreenToWorldPoint(touchPos2D);
            touchPos3D.x = touchPos3D.y * 2 + 8;
        }
        else
        {
            mousePos2D = Input.mousePosition;
            mousePos2D.z = -Camera.main.transform.position.z;
            mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        }

        inputPos = mousePos3D + touchPos3D;
        //if (accelerometrDir.sqrMagnitude > 1) accelerometrDir.Normalize();
        //accelerometrDir *= Time.deltaTime;
        
        pos = this.transform.position;
        //pos.x = Mathf.Lerp(pos.x, mousePos3D.x, alpha);
        pos.x = Mathf.Lerp(pos.x, inputPos.x, alpha);
        



        if (basketHP < 1)
        {
            DestroyBasket();
        }
        //float x = 0;
        ////float translation = 10;
        //float translation = Input.GetAxis("Horizontal") * speed;
        //x = transform.position.x;
        //translation *= Time.deltaTime;
        //if (x < 9 && translation > 0) transform.Translate(translation, 0, 0);
        //if (x > -9 && translation < 0) transform.Translate(translation, 0, 0);
    }

    private void FixedUpdate()
    {
        this.transform.position = pos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            GameObject apple = other.gameObject;
            Apple aScript = apple.GetComponent<Apple>();
            string t = apple.tag;
        
            if (t == "Apple")
            {
                score = int.Parse(scoreGT.text);
                score += aScript.cost;
                scoreGT.text = score.ToString();

                if (score > HighScore.score) HighScore.score = score;

                if (aScript.badApple) DestroyBasket();

                if (aScript.heal > 0)
                {
                    ApplePicker apPicker = Camera.main.GetComponent<ApplePicker>();
                    apPicker.BasketHeal(aScript.heal);
                }
                if (aScript.addBasket)
                {
                    ApplePicker apPicker = Camera.main.GetComponent<ApplePicker>();
                    if (apPicker.numBaskets + 1 > apPicker.lScript.Count)
                    {
                        int i = apPicker.lScript.Count;
                        apPicker.BasketCreator(i);
                    }
                }
                Destroy(apple);
            }

        }
    }

    private void DestroyBasket()
    {
        ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
        int i = apScript.lScript.Count - 1;
        apScript.lScript.RemoveAt(i);
        if(i == 0)
        {
            Time.timeScale = 0;
            apScript.MenuOnOff(true, 0f);
        }
        if (apScript.lBaskets[i] != null)
        {
            apScript.lBasketSL[i].value = 0;
            Destroy(apScript.lBasketSL[i].gameObject);
            Destroy(apScript.lBaskets[i]);
            apScript.lBaskets.RemoveAt(i);
            apScript.lBasketSL.RemoveAt(i);
        }
    }
}
