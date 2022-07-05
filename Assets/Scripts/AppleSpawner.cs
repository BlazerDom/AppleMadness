using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppleSpawner : MonoBehaviour
{
    public GameObject[] leavesSpawnerGO;
    public AudioClip[] treeOpSounds;
    private GameObject thatAppleGO;
    public GameObject appleGO;
    public Rigidbody appleRB;
    public float treeRotSpeed = 1f;
    public int spawnMin = 1;
    public int spawnMax = 3;
    public float appleVel = 500f;
    public float velScaleMin = -0.2f;
    public float velScaleMax = 1f;
    private float dirrection = 0;
    public bool gravityForApple = true;
    [HideInInspector]
    public float spawn = 0f;

    public int scoreDiffForMax = 10000;
    public int scoreDiffForMin = 10000;
    public int scoreDiffForSpeed = 100000;
    private int costil = 0;

    private Text scoreGT;
    private Animator rollingAnim;
    private AudioSource audioSorce;

    public Apples[] applesArray;

    private void Awake()
    {
        foreach (Apples a in applesArray)
        {
            a.chanceLerpValue = a.appleChance / (a.basicMaxChance - a.basicMinChance);
        }
    }
    void Start()
    {
        audioSorce = GetComponent<AudioSource>();
        rollingAnim = GetComponentInChildren<Animator>();
        rollingAnim.speed = treeRotSpeed;
        rollingAnim.Play("Rolling", -1, 0.5f);
        leavesSpawnerGO = GameObject.FindGameObjectsWithTag("AppleSpawner");
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        scoreGT = scoreGO.GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int i = int.Parse(scoreGT.text);
        if ( i > scoreDiffForMax)
        {
            spawnMax += spawnMin - 1;
            scoreDiffForMax *= 2;
        }
        
        if (i > scoreDiffForSpeed)
        {
            treeRotSpeed *= 1.15f;
            scoreDiffForSpeed *= 2;
            rollingAnim.speed = treeRotSpeed;
        }

        if(i > scoreDiffForMin)
        {
            spawnMin += 1;
            scoreDiffForMin *= 2;
        }

    }

    public void Event_SpawnAppleRight()
    {
        if(costil == 1)
        {
            costil = 0;
            int i = Random.Range(0, treeOpSounds.Length);
            AppleCreator(velScaleMin, velScaleMax);
            rollingAnim.SetFloat("Dirrection", 1);
            if (i < 3) rollingAnim.Play("OpaState", 1, 0);
            else rollingAnim.Play("OpState", 1, 0);
            audioSorce.pitch = Random.Range(0.9f, 1.1f);
            audioSorce.PlayOneShot(treeOpSounds[i]);
        }
    }

    public void Event_SpawnAppleLeft()
    {
        if(costil == 0)
        {
            costil = 1;
            int i = Random.Range(0, treeOpSounds.Length);
            AppleCreator(-velScaleMax, -velScaleMin);
            rollingAnim.SetFloat("Dirrection", -1);
            rollingAnim.SetLayerWeight(1, 1f);
            if (i < 3) rollingAnim.Play("OpaState", 1, 0);
            else rollingAnim.Play("OpState", 1, 0);
            audioSorce.pitch = Random.Range(0.9f, 1.1f);
            audioSorce.PlayOneShot(treeOpSounds[i]);
        }
    }
    private void AppleCreator(float minF, float maxF)
    {
        spawn = Random.Range(spawnMin, spawnMax);
        for(int i=0; i <= spawn; i++)
        {
            int spawnPointIndex = Random.Range(0, leavesSpawnerGO.Length);
            dirrection = Random.Range(minF, maxF);

            thatAppleGO = Instantiate<GameObject>(appleGO);
            Apple ap = thatAppleGO.GetComponent<Apple>();
            ap.appleSpawnerScript = GetComponent<AppleSpawner>();
            thatAppleGO.SetActive(true);
            thatAppleGO.transform.SetPositionAndRotation(leavesSpawnerGO[spawnPointIndex].transform.position, new Quaternion (0,0, Random.Range(-0.1f, 0.1f), 1));
            thatAppleGO.transform.parent = leavesSpawnerGO[spawnPointIndex].transform;

            appleRB = thatAppleGO.GetComponent<Rigidbody>();
            appleRB.useGravity = gravityForApple;

            StartCoroutine(WaitForApple(thatAppleGO, ap, appleRB, dirrection, 2.2f / treeRotSpeed));            
        }
    }

    IEnumerator WaitForApple(GameObject thatAppleGO, Apple ap, Rigidbody appleRB, float dirrection, float i)
    {
        yield return new WaitForSeconds(i);
        thatAppleGO.transform.parent = null;
        appleRB.isKinematic = false;
        appleRB.AddForce(new Vector3(dirrection, Random.Range(1f, 1f), 0) * appleVel);
        appleRB.AddTorque(Random.insideUnitSphere * 5f);
        var appleName = ap.appleName;

        //Циклы для увелечения и уменьшения шанса выпадения уникальных яблок
        if (appleName == "Apple")
        {
            //Если уникальное яблоко не выпадет, то у него увеличивется шанс выпадения
            foreach (Apples apple in applesArray)
            {
                if (apple.chanceLerpValue < 1) apple.chanceLerpValue += treeRotSpeed / (spawnMax * 10);
                if (apple.chanceLerpValue > 1) apple.chanceLerpValue = 1;
                apple.appleChance = Mathf.Lerp(apple.basicMinChance, apple.basicMaxChance, apple.chanceLerpValue);
            }
        }
        else
        {
            //Если уникальное яблоко выпадает, то у него уменьшается шанс выпадения
            foreach (Apples apple in applesArray)
            {
                if (appleName == apple.appleName)
                {
                    if (apple.chanceLerpValue > 0) apple.chanceLerpValue = 0f;
                    apple.appleChance = Mathf.Lerp(apple.basicMinChance, apple.basicMaxChance, apple.chanceLerpValue);
                }
                else
                {
                    if (apple.chanceLerpValue < 1) apple.chanceLerpValue += 0.1f;
                    apple.appleChance = Mathf.Lerp(apple.basicMinChance, apple.basicMaxChance, apple.chanceLerpValue);
                }
            }
        }
    }

    [System.Serializable]
    public class Apples
    {
        public string appleName = "";
        public Material appleSkin;
        public Color appleColor;

        public float appleChance = 0f;

        [HideInInspector]
        public float chanceLerpValue = 0.5f;
        public float basicMinChance = 0f;
        public float basicMaxChance = 0f;

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
