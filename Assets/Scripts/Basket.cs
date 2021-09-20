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

    public static Text scoreGT;

    public Vector3 pos = Vector3.zero;

    public AudioClip[] catchSounds;
    private AudioSource audioSource;

    public Vector3 standartScale= Vector3.zero;
    private ApplePicker apPicker;
    public bool improveBasket = false;

    private void Start()
    {
        ApplePicker apPicker = Camera.main.GetComponent<ApplePicker>();
        standartScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
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
        apPicker = Camera.main.GetComponent<ApplePicker>();

        if (other.gameObject != null)
        {
            GameObject apple = other.gameObject;
            Apple aScript = apple.GetComponent<Apple>();
            string t = apple.tag;
        
            if (t == "Apple")
            {
                apPicker.successCatchs += 1;
                if (apPicker.successCatchs == 4) apPicker.scoreMultipler = 2;
                if (apPicker.successCatchs == 8) apPicker.scoreMultipler = 4;
                if (apPicker.successCatchs == 16) apPicker.scoreMultipler = 8;

                audioSource.PlayOneShot(catchSounds[Random.Range(0, catchSounds.Length)]);
                score = int.Parse(scoreGT.text);
                score += aScript.cost * apPicker.scoreMultipler;
                scoreGT.text = score.ToString();


                if (score > HighScore.score) HighScore.score = score;

                if (aScript.badApple)
                {
                    apPicker.successCatchs = 0;
                    apPicker.scoreMultipler = 1;
                    DestroyBasket();
                }
                

                if (aScript.heal > 0)
                {
                    apPicker.BasketHeal(aScript.heal);
                }
                if (aScript.addBasket)
                {
                    if (apPicker.numBaskets + 1 == apPicker.lScript.Count)
                    {
                        for (int i = 0; i < apPicker.lBaskets.Count; i++)
                        {
                            Vector3 s = apPicker.lBaskets[i].transform.localScale;
                            if (s == apPicker.lScript[i].standartScale)
                            {
                                apPicker.lScript[i].improveBasket = true;
                                apPicker.lBaskets[i].transform.localScale = new Vector3(s.x + 1.5f, s.y, s.z);
                            }
                        }
                    }
                    if (apPicker.numBaskets + 1 > apPicker.lScript.Count)
                    {
                        int i = apPicker.lScript.Count;
                        apPicker.BasketCreator(i);
                    }
                }
                Destroy(apple, 1.1f);
                MeshRenderer[] mr = apple.GetComponentsInChildren<MeshRenderer>();
                Rigidbody rb = apple.GetComponent<Rigidbody>();
                SphereCollider sc = apple.GetComponent<SphereCollider>();
                sc.enabled = false;
                rb.isKinematic = true;
                for(int i = 0; i < mr.Length; i++)
                {
                    mr[i].enabled = false;
                }
            }

        }
    }

    private void DestroyBasket()
    {
        apPicker = Camera.main.GetComponent<ApplePicker>();
        int i = apPicker.lScript.Count - 1;
        if (i == 0)
        {
            apPicker.gameOver = true;
            apPicker.MenuOnOff();
        }
        if (apPicker.lBaskets[i] != null)
        {
            if (apPicker.lScript[i].improveBasket)
            {
                for (int j = 0; j < apPicker.lBaskets.Count; j++)
                {
                    Vector3 s = apPicker.lBaskets[j].transform.localScale;
                    if (s.x != apPicker.lScript[j].standartScale.x)
                    {
                        apPicker.lScript[j].improveBasket = false;
                        apPicker.lBaskets[j].transform.localScale = new Vector3(s.x - 1.5f, s.y, s.z);
                    }
                }

            }
            else
            {
                apPicker.lScript.RemoveAt(i);
                apPicker.lBasketSL[i].value = 0;
                Destroy(apPicker.lBasketSL[i].gameObject);
                Destroy(apPicker.lBaskets[i]);
                apPicker.lBaskets.RemoveAt(i);
                apPicker.lBasketSL.RemoveAt(i);
            }
        }
    }

    private void BasketScale(float f, bool b)
    {
        for (int i = 0; i < apPicker.lBaskets.Count; i++)
        {
            Vector3 s = apPicker.lBaskets[i].transform.localScale;
            if (s == apPicker.lScript[i].standartScale)
            {
                apPicker.lScript[i].improveBasket = b;
                apPicker.lBaskets[i].transform.localScale = new Vector3(s.x + f, s.y, s.z);
            }
        }
    }
}
