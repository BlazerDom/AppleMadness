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

    public List<GameObject> lBaskets;
    public List<Basket> lScript;
    public List<Slider> lBasketSL;

    public bool gameOver = false;

    public GameObject menu;

    public Slider basketSliderPrefab;

    public KeyCode mainMenu;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        Text scoreGT = scoreGO.GetComponent<Text>();
        scoreGT.text = "0";

        lBaskets = new List<GameObject>();
        lScript = new List<Basket>();
        lBasketSL = new List<Slider>();

        for(int i = 0; i < numBaskets; i++)
        {
            BasketCreator(i);
            //GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
            //GameObject canvas = GameObject.Find("FirstCanvas");
            //Slider basketSL = Instantiate<Slider>(basketSliderPrefab, canvas.transform);
            //basketSL = basketSL.GetComponent<Slider>();
            
            //lBasketSL.Add(basketSL);
            //lBaskets.Add(tBasketGO);
            //lScript.Add(tBasketGO.GetComponent<Basket>());
            //lScript[i].basketHP = basketHP;
            //Vector3 pos = Vector3.zero;
            //pos.y = basketBottomY + (basketSpacingY * i);
            //tBasketGO.transform.position = pos;
            //lScript[i].alpha = 1f / (i*25f);
            //basketSL.transform.position -= new Vector3(0, 20 * i); 
        }
    }

    public void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        MenuOnOff();
    }

    private void Update()
    {
        if (Input.GetKeyDown(mainMenu))
        {
            MenuOnOff();
        }
    }

    public void AppleDestroyed(GameObject apple, int damage)
    {
        int i = lScript.Count - 1;
        MeshRenderer mr = apple.GetComponentInChildren<MeshRenderer>();
        Rigidbody rb = apple.GetComponent<Rigidbody>();        
        rb.isKinematic = true;
        mr.enabled = false;

        Destroy(apple, 1.1f);
        lScript[i].basketHP -= damage;
        lBasketSL[i].value = (float)lScript[i].basketHP / (float)basketHP;
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
        menu.gameObject.SetActive(!menu.gameObject.activeSelf);

        if (menu.gameObject.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        
    }

    public void BasketCreator(int i)
    {
        GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
        GameObject canvas = GameObject.Find("FirstCanvas");
        Slider basketSL = Instantiate<Slider>(basketSliderPrefab, canvas.transform);
        basketSL = basketSL.GetComponent<Slider>();

        lBasketSL.Add(basketSL);
        lBaskets.Add(tBasketGO);
        lScript.Add(tBasketGO.GetComponent<Basket>());
        lScript[i].basketHP = basketHP;
        lScript[i].pos = Vector3.zero;
        lScript[i].pos.y = basketBottomY + (basketSpacingY * i);
        tBasketGO.transform.position = lScript[i].pos;
        lScript[i].alpha = 1f / (i * basketStretch);
        basketSL.transform.position -= new Vector3(0, 20 * i);
    }
}
