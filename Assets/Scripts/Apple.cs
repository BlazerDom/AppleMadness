using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public static float bottomY = -14f;

    public int cost = 100;

    public int damage = 2;

    public bool badApple = false;

    public int heal = 0;

    public bool addBasket = false;

    public float bounce = 100f;

    public float seedForce = 75f;

    public Material[] appleMaterials;
    public GameObject goodAppleGO;
    public GameObject badAppleGO;
    public GameObject seedGO;

    private float redAppleChance = Mathf.Clamp(.1f, 0f, 1f);
    private float healAppleChance = Mathf.Clamp(.05f, 0f, 1f);
    private float badAppleChance = Mathf.Clamp(.03f, 0f, 1f);
    private float resurectAppleChance = Mathf.Clamp(.01f, 0f, 1f);
    private float jumpingAppleChance = Mathf.Clamp(.02f, 0f, 1f);

    public AudioClip[] greenAppleSound;
    public AudioClip[] redAppleSound;
    public AudioClip[] healAppleSound;
    public AudioClip[] badAppleSound;
    public AudioClip[] resurectAppleSound;

    private Rigidbody rb;
    private AudioSource audioSource;
    private int soundPlayInt = 0;
    private bool death = false;
    private Quaternion rot = new Quaternion(0f, 0f, 0f, 1f);
    private bool isJumping = false;

    public void Start()
    {
        ApplePicker ap = Camera.main.GetComponent<ApplePicker>();
        redAppleChance = ap.redAppleChance;
        healAppleChance = ap.healAppleChance;
        badAppleChance = ap.badAppleChance;
        resurectAppleChance = ap.resurectAppleChance;
        jumpingAppleChance = ap.jumpingAppleChance;
        bounce = ap.appleBounce;


        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        gameObject.transform.localScale = Vector3.zero;
        WaitForLaunch();
        MeshRenderer mr = goodAppleGO.GetComponentInChildren<MeshRenderer>();
        float i = Random.value;
        float j = 0f;
        audioSource.clip = greenAppleSound[Random.Range(0, greenAppleSound.Length)];

        //Red Apple chances
        if (i != 0 && i > j && i < j + redAppleChance)
        {
            cost = 400;            
            mr.material = appleMaterials[1];
            damage = 4;
            audioSource.clip = redAppleSound[Random.Range(0, redAppleSound.Length)];
            audioSource.priority = 122;
        }

        j += redAppleChance;

        //Bad Apple chances
        if (i != 0 && i > j && i < j + badAppleChance)
        {
            cost = 0;
            goodAppleGO.SetActive(false);
            badAppleGO.SetActive(true);           
            badApple = true;
            damage = 0;
            audioSource.clip = badAppleSound[Random.Range(0, badAppleSound.Length)];
            audioSource.volume = 1;
            audioSource.priority = 119;
        }

        j += badAppleChance;

        //Heal Apple chances
        if (i != 0 && i > j && i < j + healAppleChance)
        {
            cost = 600;
            mr.material = appleMaterials[2];
            heal = 6;
            damage = 1;
            audioSource.clip = healAppleSound[Random.Range(0, healAppleSound.Length)];
            audioSource.volume = 0.8f;
            audioSource.priority = 121;
        }

        //add basket Apple chances
        j += healAppleChance; 
        if(i != 0 && i > j && i < j + resurectAppleChance)
        {
            cost = 1000;
            mr.material = appleMaterials[3];
            damage = 6;
            addBasket = true;
            audioSource.clip = resurectAppleSound[Random.Range(0, resurectAppleSound.Length)];
            audioSource.volume = 0.8f;
            audioSource.priority = 110;
            TrailRenderer tr = gameObject.GetComponent<TrailRenderer>();
            tr.enabled = true;
        }

        //add Jumping Apple chances
        j += resurectAppleChance;
        if(i != 0 && i > j && i < j + jumpingAppleChance)
        {
            cost = 1000;
            mr.material = appleMaterials[4];
            damage = 8;
            Jumping();
        }
    }

    private void Update()
    {
        if (transform.position.y < bottomY && !death)
        {
            if(damage != 0)
            {
                Vector3 pos = transform.position;
                Vector3 rotAngle = new Vector3(0f, 0f, 90f / (damage * 2));
                rot.eulerAngles = new Vector3(0f, 0f, 303.75f + rotAngle.z);
                for (int i = 0; i < damage * 2; i++)
                {
                    //if (i < damage / 2) forceY *= 1.5f;

                    //if (i > damage / 2) forceY /= 1.5f;
                    GameObject s = Instantiate(seedGO, pos, rot);
                    s.transform.parent = null;

                    s.transform.rotation = rot;
                    //rot.eulerAngles = s.transform.rotation.eulerAngles + new Vector3 (0f, 0f, 90f) / (damage * 2);
                    //rot = Quaternion.AngleAxis(90f / (damage * 2), s.transform.forward);
                    s.transform.Rotate(rotAngle * i);
                    Rigidbody rs = s.GetComponent<Rigidbody>();
                    rs.AddForce(rs.transform.up * seedForce);
                    //float j = 640f / damage;
                    //forceX += j;
                }

            }
            death = true;
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
            apScript.AppleDestroyed(this.gameObject, damage);
        }
        if (!rb.isKinematic && soundPlayInt == 0)
        {
            soundPlayInt = 1;
            audioSource.Play();
        }
    }

    private void WaitForLaunch()
    {
        transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);

        if (transform.localScale.x < 1f) Invoke("WaitForLaunch", 0.1f);
    }

    private void OnCollisionEnter(Collision other)
    {
        rb.AddForce(other.contacts[0].normal * bounce);
    }

    private void Jumping()
    {
        if (transform.position.y > -10)
        {
            float x = Random.Range(-400f, 400f);
            float y = Random.Range(0f, 220f);
            float t = Random.Range(0.2f, 0.6f);
            //Vector2 rc = Random.insideUnitCircle * 400;
            rb.AddForce(new Vector3(x, y));
            Invoke("Jumping", t);
        }
    }
}
