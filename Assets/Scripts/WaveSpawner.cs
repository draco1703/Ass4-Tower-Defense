using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public int EnemisAlive = 0;
    public static WaveSpawner instance;

    public Transform enemyPrefab;
    public Transform bossPrefab;
    //public Wave[] waves;

    public Transform spawnPoint;

    public Text waveCountDownText;
    public float timeBetweenWaves = 5f;
    private float countdwon = 10f;
    public int waveIndex = 1;
    private float BossHealth = 350;
    private float health = 100;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (EnemisAlive > 0)
        {
            return;
        }

        if (countdwon <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdwon = timeBetweenWaves;
            return;
        }
        countdwon -= Time.deltaTime;
        
        countdwon = Mathf.Clamp(countdwon, 0f, Mathf.Infinity);
        waveCountDownText.text = string.Format("{0:00.00}", countdwon);


    }
    IEnumerator SpawnWave()
    {
        PlayerStats.Rounds++;

        //Wave wave = waves[waveIndex]
        if (waveIndex % 7 != 0)
        {
            EnemisAlive = waveIndex;
            for (int i = 0; i < waveIndex; i++)
            {

                SpawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }
            //not in yet
            health += 20;

        }
        else
        {
            int amount = waveIndex / 7;
            EnemisAlive = amount;
            for (int i = 0; i < amount; i++)
            {
                SpawnBoss();
                yield return new WaitForSeconds(0.5f);
            }
            //not in yet
            BossHealth *= 3f;
        }

        
        
        waveIndex++;
    
    }

    //should had make to one poll and then reset the stats
    void SpawnEnemy()
    {
        
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        
    }
    void SpawnBoss()
    {
        Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
        
    }
}
