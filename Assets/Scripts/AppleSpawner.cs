using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppleSpawner : MonoBehaviour
{
    public GameObject[] leavesSpawnerGO;
    private GameObject thatAppleGO;
    public GameObject appleGO;
    public Rigidbody appleRB;
    public float treeRotSpeed = 1f;
    public int spawnMax = 3;
    public float appleMaxVel = 500f;
    public float appleMinVel = 300f;
    private float dirrection = 0;

    public int scoreDifficult = 10000;
    public int scoreDifficultTwo = 100000;

    private Text scoreGT;
    private Animator rollingAnim;
    // Start is called before the first frame update
    void Start()
    {
        rollingAnim = GetComponent<Animator>();
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
        if ( i > scoreDifficult)
        {
            spawnMax += 1;
            scoreDifficult *= 2;

            if (i > scoreDifficultTwo)
            {
                treeRotSpeed *= 2;
                scoreDifficultTwo *= 3;
            }
        }
    }

    private void Event_SpawnAppleRight()
    {
        AppleCreator(0.02f, 0.5f);
        rollingAnim.SetFloat("Dirrection", 1);
    }

    private void Event_SpawnAppleLeft()
    {
        AppleCreator(-0.5f, -0.02f);
        rollingAnim.SetFloat("Dirrection", -1);

    }
    private void AppleCreator(float minF, float maxF)
    {
        int spawn = Random.Range(1, spawnMax);
        for(int i=0; i < spawn; i++)
        {
            int spawnPointIndex = Random.Range(0, leavesSpawnerGO.Length);
            dirrection = Random.Range(minF, maxF);

            thatAppleGO = Instantiate<GameObject>(appleGO);
            thatAppleGO.transform.position = leavesSpawnerGO[spawnPointIndex].transform.position + new Vector3(1, Random.Range(-1, 1), Random.Range(-1, 1));
            thatAppleGO.transform.parent = leavesSpawnerGO[spawnPointIndex].transform;

            appleRB = thatAppleGO.GetComponent<Rigidbody>();

            StartCoroutine(WaitForLaunch(thatAppleGO, appleRB, dirrection, 2.2f / treeRotSpeed));            
        }
    }

    IEnumerator WaitForLaunch(GameObject thatAppleGO, Rigidbody appleRB, float dirrection, float i)
    {
        yield return new WaitForSeconds(i);
        thatAppleGO.transform.parent = null;
        appleRB.isKinematic = false;
        appleRB.AddForce(new Vector3(dirrection, Random.Range(0, 0.5f), 0) * Random.Range(appleMinVel, appleMaxVel));
    }
}
