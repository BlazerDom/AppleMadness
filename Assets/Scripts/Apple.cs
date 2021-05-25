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

    public float bounce = 100f;

    public Material[] appleMaterials;
    public GameObject goodAppleGO;
    public GameObject badAppleGO;

    private float redAppleChance = Mathf.Clamp(.1f, 0f, 1f);
    private float healAppleChance = Mathf.Clamp(.05f, 0f, 1f);
    private float badAppleChance = Mathf.Clamp(.03f, 0f, 1f);
    private float resurectAppleChance = Mathf.Clamp(.01f, 0f, 1f);

    public AudioClip[] greenAppleSound;
    public AudioClip[] redAppleSound;
    public AudioClip[] healAppleSound;
    public AudioClip[] badAppleSound;
    public AudioClip[] resurectAppleSound;

    private Rigidbody rb;
    private AudioSource audioSource;
    private int soundPlayInt = 0;
    private bool death = false;

    public void Start()
    {
        ApplePicker ap = Camera.main.GetComponent<ApplePicker>();
        redAppleChance = ap.redAppleChance;
        healAppleChance = ap.healAppleChance;
        badAppleChance = ap.badAppleChance;
        resurectAppleChance = ap.resurectAppleChance;


        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        gameObject.transform.localScale = Vector3.zero;
        WaitForLaunch();
        MeshRenderer mr = goodAppleGO.GetComponentInChildren<MeshRenderer>();
        float i = Random.value;
        float j = 0f;
        audioSource.clip = greenAppleSound[Random.Range(0, greenAppleSound.Length)];

        //Red Apple chances
        if (i > j && i < j + redAppleChance)
        {
            cost = 400;            
            mr.material = appleMaterials[1];
            damage = 4;
            audioSource.clip = redAppleSound[Random.Range(0, redAppleSound.Length)];
            audioSource.priority = 122;
        }

        j += redAppleChance;

        //Bad Apple chances
        if (i > j && i < j + badAppleChance)
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
        if (i > j && i < j + healAppleChance)
        {
            cost = 600;
            mr.material = appleMaterials[2];
            heal = 6;
            damage = 0;
            audioSource.clip = healAppleSound[Random.Range(0, healAppleSound.Length)];
            audioSource.volume = 0.8f;
            audioSource.priority = 121;
        }

        //add basket Apple chances
        j += healAppleChance; 
        if(i > j && i < j + resurectAppleChance)
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
    }

    private void Update()
    {
        if (transform.position.y < bottomY && !death)
        {
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
}
