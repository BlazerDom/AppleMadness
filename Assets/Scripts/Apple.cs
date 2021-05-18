using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public static float bottomY = -18f;

    public int cost = 100;

    public int damage = 2;

    public bool badApple = false;

    public int heal = 0;

    public bool addBasket = false;

    public float redAppleChance = Mathf.Clamp(.1f, 0f, 1f);
    public float HealAppleChance = Mathf.Clamp(.05f, 0f, 1f);
    public float badAppleChance = Mathf.Clamp(.03f, 0f, 1f);
    public float resurectAppleChance = Mathf.Clamp(.01f, 0f, 1f);
    public void Start()
    {
        gameObject.transform.localScale = Vector3.zero;

        WaitForLaunch();
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        float i = Random.value;
        float j = 0f;

        if (i > j && i < j + redAppleChance)
        {
            cost = 500;            
            mr.material.color = Color.red;
            damage = 4;
        }

        j += redAppleChance;

        if (i > j && i < j + badAppleChance)
        {
            cost = 10000;
            mr.material.color = Color.black;
            badApple = true;
            damage = 0;
        }

        j += badAppleChance;

        if (i > j && i < j + HealAppleChance)
        {
            cost = 500;
            mr.material.color = Color.yellow;
            heal = 6;
            damage = 0;
        }

        j += HealAppleChance; 
        if(i > j && i < j + resurectAppleChance)
        {
            cost = 0;
            mr.material.color = Color.blue;
            addBasket = true;
        }
    }

    private void Update()
    {
        if (transform.position.y < bottomY)
        {
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
            apScript.AppleDestroyed(this.gameObject, damage);
        }
    }

    private void WaitForLaunch()
    {
        transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

        if (transform.localScale.x < 0.3f) Invoke("WaitForLaunch", 0.1f);
    }
}
