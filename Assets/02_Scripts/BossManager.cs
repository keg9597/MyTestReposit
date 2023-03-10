using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    //public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public GameObject[] enemyPool;
    public GameObject bossInstance;

    public int poolSize = 3;

    public float curTime;
    public float coolTime = 2;

    int spawnCnt = 1;
    public PlayerMovement playerState;
    public GameObject[] spawnPositions;

    public float bossSpawnDistance = 10f;
    bool hasSpawnedBoss = false;

    void Start()
    {
        playerState = GameObject.Find("Player").GetComponent<PlayerMovement>();
        enemyPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            //enemyPool[i] = Instantiate(enemyPrefab);
            //enemyPool[i].SetActive(false);
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
            //for (int i = 0; i < enemyPool.Length; i++)
            //{
            //    if (enemyPool[i].activeSelf == true)
            //        continue;

            //    float x = Random.Range(-20, 20);
            //    int rndPos = Random.Range(0, spawnPositions.Length);
            //    enemyPool[i].transform.position = spawnPositions[rndPos].transform.position;
            //    enemyPool[i].SetActive(true);
            //    enemyPool[i].name = "ENEMY" + spawnCnt;
            //    ++spawnCnt;
            //    break;
            //}
        }

        // Spawn boss if player is close enough
        if (!hasSpawnedBoss && Vector3.Distance(transform.position, playerState.transform.position) < bossSpawnDistance)
        {
            bossInstance = Instantiate(bossPrefab);
            bossInstance.SetActive(true);
            bossInstance.name = "BOSS";
            hasSpawnedBoss = true;
        }
    }
}

