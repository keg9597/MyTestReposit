using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] enemyPool;
    public int poolSize = 3;

    public float curTime;
    public float coolTime = 2;

    int spawnCnt = 1;
    public PlayerMovement playerState;
    public GameObject[] spawnPositions;

    void Start()
    {
        playerState = GameObject.Find("Player").GetComponent<PlayerMovement>();
        enemyPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            enemyPool[i] = Instantiate(enemyPrefab);
            enemyPool[i].SetActive(false);
        }
        spawnPositions = GameObject.FindGameObjectsWithTag("SpawnPos");
    }


    void Update()
    {
        if (playerState.isDead)
            return;

        curTime += Time.deltaTime;

        if (curTime > coolTime)
        {
            curTime = 0;
            for (int i = 0; i < enemyPool.Length; i++)
            {
                if (enemyPool[i].activeSelf == true)
                    continue;

                float x = Random.Range(-20, 20);
                int rndPos = Random.Range(0, spawnPositions.Length);
                //enemyPool[i].transform.position = new Vector3(x, 1, 20f);
                enemyPool[i].transform.position = spawnPositions[rndPos].transform.position;
                enemyPool[i].SetActive(true);
                enemyPool[i].name = "ENEMY" + spawnCnt;
                ++spawnCnt;
                break;
            }
        }
    }
}
