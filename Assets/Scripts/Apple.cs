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

    public bool gravityField = false;

    //public Material[] appleMaterials;
    public GameObject goodAppleGO;
    public GameObject badAppleGO;
    public GameObject seedGO;

    //private float redAppleChance = Mathf.Clamp(.1f, 0f, 1f);
    //private float healAppleChance = Mathf.Clamp(.05f, 0f, 1f);
    //private float badAppleChance = Mathf.Clamp(.03f, 0f, 1f);
    //private float resurectAppleChance = Mathf.Clamp(.01f, 0f, 1f);
    //private float jumpingAppleChance = Mathf.Clamp(.02f, 0f, 1f);

    public AudioClip[] greenAppleSound;
    //public AudioClip[] redAppleSound;
    //public AudioClip[] healAppleSound;
    //public AudioClip[] badAppleSound;
    //public AudioClip[] resurectAppleSound;

    private new Rigidbody rigidbody;
    //private Collider gravityColl;
    private AudioSource audioSource;
    private int soundPlayInt = 0;
    private bool death = false;
    private Quaternion rot = new Quaternion(0f, 0f, 0f, 1f);
    private bool isJumping = false;
    private float gravityRange = 100;

    public Apples[] applesArray;
    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        gameObject.transform.localScale = Vector3.zero;
        WaitForLaunch();
        MeshRenderer mr = goodAppleGO.GetComponentInChildren<MeshRenderer>();
        float r = Random.value;
        float j = 0f;
        audioSource.clip = greenAppleSound[Random.Range(0, greenAppleSound.Length)];
        for (int i = 0; i < applesArray.Length; i++)
        {
            if (i > 0) j += applesArray[i - 1].appleChance;
            if (r != 0 && r > j && r < j + applesArray[i].appleChance)
            {
                mr.material = applesArray[i].appleSkin;
                cost = applesArray[i].cost;
                damage = applesArray[i].damage;
                heal = applesArray[i].heal;
                badApple = applesArray[i].badApple;
                addBasket = applesArray[i].addBasket;
                gravityField = applesArray[i].gravityField;
                isJumping = applesArray[i].isJumping;

                if(badApple == true)
                {
                    goodAppleGO.SetActive(false);
                    badAppleGO.SetActive(true);           
                }

                if(addBasket == true)
                {
                    TrailRenderer tr = gameObject.GetComponent<TrailRenderer>();
                    tr.enabled = true;
                }
                if (isJumping == true) Jumping();

                audioSource.clip = applesArray[i].sounds[Random.Range(0, applesArray[i].sounds.Length)];
                audioSource.priority = applesArray[i].soundPriority;
                audioSource.volume = applesArray[i].soundVolume;

            }
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
        if (!rigidbody.isKinematic && soundPlayInt == 0)
        {
            soundPlayInt = 1;
            audioSource.Play();
        }
    }

    private void FixedUpdate()
    {
        GravityApple(gravityField);
    }
    private void WaitForLaunch()
    {
        transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);

        if (transform.localScale.x < 1f) Invoke("WaitForLaunch", 0.1f);
    }

    private void GravityApple(bool gravity)
    {
        if (gravity)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, gravityRange);
            List<Rigidbody> rbs = new List<Rigidbody>();

            foreach (Collider c in cols)
            {
                Rigidbody rb = c.attachedRigidbody;
                if (rb != null && rb != rigidbody && !rbs.Contains(rb))
                {
                    rbs.Add(rb);
                    rb.useGravity = false;
                    Vector3 offset = transform.position - c.transform.position;
                    rb.AddForce(offset / offset.magnitude * rigidbody.mass);
                }
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        rigidbody.AddForce(other.contacts[0].normal * bounce);
    }

    private void Jumping()
    {
        if (transform.position.y > -10)
        {
            float x = Random.Range(-400f, 400f);
            float y = Random.Range(0f, 220f);
            float t = Random.Range(0.2f, 0.6f);
            //Vector2 rc = Random.insideUnitCircle * 400;
            rigidbody.AddForce(new Vector3(x, y));
            Invoke("Jumping", t);
        }
    }

    [System.Serializable]
    public class Apples
    {
        public string appleName = "";
        public Material appleSkin;
        public float appleChance = 0f;
        public AudioClip[] sounds;
        public int soundPriority;
        public float soundVolume;

        public int cost = 0;
        public int damage = 2;
        public int heal = 0;

        public bool badApple = false;        
        public bool addBasket = false;
        public bool gravityField = false;
        public bool isJumping = false;

        public float bounce = 100f;
        public float seedForce = 75f;        
    }
}
