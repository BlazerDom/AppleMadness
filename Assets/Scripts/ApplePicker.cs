using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour
{
    [Header("Set In Inspector")]
    public GameObject basketPrefab;
    public int numBaskets = 3;
    public float basketBottomY = -10f;
    public float basketSpacingY = -1f;
    public int basketHP = 10;
    public float basketStretch = 10;

    [Header("Set Apple Chances")]
    public float redAppleChance = Mathf.Clamp(.1f, 0f, 1f);
    public float healAppleChance = Mathf.Clamp(.05f, 0f, 1f);
    public float badAppleChance = Mathf.Clamp(.03f, 0f, 1f);
    public float resurectAppleChance = Mathf.Clamp(.01f, 0f, 1f);
    public float jumpingAppleChance = Mathf.Clamp(.02f, 0f, 1f);
    public float appleBounce = 100f;

    public List<GameObject> lBaskets;
    public List<Basket> lScript;
    public List<Slider> lBasketSL;

    public bool gameOver = false;

    public GameObject menu;

    public Slider basketSliderPrefab;

    public KeyCode mainMenu;

    private GameObject startButton;
    private bool firstLaunch = true;

    public int successCatchs = 0;
    public int scoreMultipler = 1;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        Text scoreGT = scoreGO.GetComponent<Text>();
        scoreGT.text = "0";

        GameObject a = menu.transform.GetChild(0).gameObject;
        GameObject b = a.transform.GetChild(0).gameObject;
        startButton = b.transform.GetChild(1).gameObject;


        lBaskets = new List<GameObject>();
        lScript = new List<Basket>();
        lBasketSL = new List<Slider>();

        for(int i = 0; i < numBaskets; i++)
        {
            BasketCreator(i);
        }
    }

    public void Start()
    {
        if (Camera.main.aspect > 1.6f) Camera.main.orthographicSize = 13;
        if (Camera.main.aspect <= 1.6f) Camera.main.orthographicSize = 15.3f;
        MenuOnOff();
    }

    private void Update()
    {
        if (Input.GetKeyDown(mainMenu))
        {
            MenuOnOff();
        }

        SkyboxRotator();
    }

    public void AppleDestroyed(GameObject apple, int damage)
    {
        //int i = lScript.Count - 1;
        //if (i < 0) return;
        MeshRenderer mr = apple.GetComponentInChildren<MeshRenderer>();
        Rigidbody rb = apple.GetComponent<Rigidbody>();
        Apple a = apple.GetComponent<Apple>();
        Collider ac = apple.GetComponent<Collider>();
        ac.enabled = false;
        rb.isKinematic = true;
        mr.enabled = false;

        Destroy(apple, 1.1f);
        //lScript[i].basketHP -= damage;
        //lBasketSL[i].value = (float)lScript[i].basketHP / (float)basketHP;
        if (!a.badApple)
        {
            successCatchs = 0;
            scoreMultipler = 1;
        }
    }

    public void SeedDamageBasket(GameObject seed, int damage)
    {
        int i = lScript.Count - 1;
        if (i < 0) return;
        Destroy(seed);
        lScript[i].basketHP -= damage;
        lBasketSL[i].value = lScript[i].basketHP / (float)basketHP;
    }

    public void BasketHeal(int heal)
    {        
        var basketNeedHeal = lScript.Where(x => x.basketHP < basketHP).LastOrDefault();
        if (basketNeedHeal == null) return;
        basketNeedHeal.basketHP = basketNeedHeal.basketHP + heal >= basketHP ? basketHP : basketNeedHeal.basketHP + heal;
        var i = lScript.IndexOf(basketNeedHeal);
        lBasketSL[i].value = (float)basketNeedHeal.basketHP / (float)basketHP;
        //int i = lScript.Count - 1;
        //lScript[i].basketHP += heal;
        //if (lScript[i].basketHP > basketHP) lScript[i].basketHP = basketHP;
        //lBasketSL[i].value = (float)lScript[i].basketHP / (float)basketHP;
    }
    public void MenuOnOff()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            menu.gameObject.SetActive(true);
            MenuButtons mbRef = menu.GetComponentInChildren<MenuButtons>();
            mbRef.LeaderboardButton();
            startButton.SetActive(false);
        }
        else
        {
            menu.gameObject.SetActive(!menu.gameObject.activeSelf);

            if (menu.gameObject.activeSelf)
            {
                Time.timeScale = 0;
                if (lBaskets != null && !firstLaunch)
                {
                    Text t = startButton.GetComponentInChildren<Text>();
                    t.text = "Continue";
                }
                else
                {
                    Text t = startButton.GetComponentInChildren<Text>();
                    t.text = "Start!";
                    firstLaunch = false;
                }
            }
            else
            {
                Time.timeScale = 1;
            }
        }        
    }

    public void BasketCreator(int i)
    {
        GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
        GameObject canvas = GameObject.Find("InterfaceCanvas");
        Slider basketSL = Instantiate<Slider>(basketSliderPrefab, canvas.transform);
        basketSL = basketSL.GetComponent<Slider>();

        lBasketSL.Add(basketSL);
        lBaskets.Add(tBasketGO);
        lScript.Add(tBasketGO.GetComponent<Basket>());

        lScript[i].basketHP = basketHP;
        lScript[i].pos = Vector3.zero;
        lScript[i].pos.y = basketBottomY + (basketSpacingY * i);
        tBasketGO.transform.position = lScript[i].pos;
        if (lBaskets.Count > 1)
        {
            var x = lBaskets[i - 1].transform.position.x;
            var y = lBaskets[i].transform.position.y;
            var z = lBaskets[i].transform.position.z;
            lBaskets[i].transform.position = new Vector3(x, y, z);
        }
        lScript[i].alpha = 1f / (i * basketStretch);
        basketSL.transform.position -= new Vector3(0, 20 * i);
    }

    public void SkyboxRotator()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time);
    }
}
