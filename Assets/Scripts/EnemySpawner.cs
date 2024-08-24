using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public List<SpawnRequest> spawnRequests=new List<SpawnRequest>();


    public Action WaveStarted;
    public Action WaveEnded;
    public Action WavesCompleted;
    [Tooltip("The delay before declaring all waves completed."),Min(0)]
    public float PostWaveDelay;

    [Tooltip("On a scale from 0-1 returns how close the current wave is to completion.")]
    public float WavePercentage()
    {
        if(spawnRequests.Count <= 0)
        {//If there are no spawn requests we assume 100% completion of  this wave. (Last wave ends and we have no requests left)
            return 1;
        }
        SpawnRequest current = spawnRequests[0];
        if (current.WaitUntilCompletion)
        {
            return (enemiesSpawnedThisWave / current.SpawnAmount)/2.0f +  (1-Mathf.Min(Mathf.Max(0,((float)transform.childCount/(float)current.SpawnAmount)),1))/2.0f;
        }
        else
        {//The wave ends when all enemies spawn
            return enemiesSpawnedThisWave / current.SpawnAmount;
        }
    }
    [Tooltip("On a scale from 0-1 returns how close this entire enemy spawner is to 100% completion.")]
    public float TotalPercentage()
    {
        return totalWavesCompleted/ totalWavesToSpawn;
    }

    [System.Serializable]
    public class SpawnRequest
    {
        [Tooltip("The enemy to spawn.")]
        public GameObject Prefab;
        [Tooltip("How many Enemies to spawn with this order.")]
        public float SpawnAmount=1;
        [Tooltip("Delay between each spawning.")]
        public float SpawnDelayTime;

        [Tooltip("How long to wait before next wave.")]
        public float PostWaveDelay;

        [Tooltip("True if the next wave must wait before this one is finished to start.")]
        public bool WaitUntilCompletion;


        [Tooltip("Should this enemy snap to its entry node when it spawns?")]
        public bool snapSpawning = true;
    }


    public void Start()
    {

        StartCoroutine(EnemySpawnerLoop());
    }

    private bool hasNoChildren()
    {
        return transform.childCount <= 0;
    }
    float enemiesSpawnedThisWave;
    float totalWavesCompleted;
    float totalWavesToSpawn;
    public IEnumerator EnemySpawnerLoop()
    {
        while (spawnRequests.Count > 0)
        {
            if(spawnRequests.Count> totalWavesToSpawn)
            {
                totalWavesToSpawn = spawnRequests.Count;
            }
            WaveStarted?.Invoke();
            SpawnRequest current = spawnRequests[0];
            enemiesSpawnedThisWave = 0;
            for (int i = 0; i < current.SpawnAmount; i++)
            {
                enemiesSpawnedThisWave++;
                Vector3 spawnPos = transform.position;
                PathNode initTarget = null;


                if (current.snapSpawning)
                {

                    initTarget = PathManager.Instance.getEntryNode();

                }
                if(initTarget!= null)
                {
                    spawnPos = initTarget.transform.position;
                }
                GameObject g = Instantiate(current.Prefab, spawnPos, transform.rotation, transform);

                if (initTarget != null)
                {
                    Pathfinder p = g.GetComponent<Pathfinder>();
                    p.currentNode = initTarget;
                }
                yield return new WaitForSeconds(current.SpawnDelayTime);
                


            }


            if (current.WaitUntilCompletion)
            {
                yield return new WaitUntil(hasNoChildren);
            }
            totalWavesCompleted++;
            WaveEnded?.Invoke();

            yield return new WaitForSeconds(current.PostWaveDelay);


            yield return new WaitForEndOfFrame();
            spawnRequests.RemoveAt(0);
        }

        while(transform.childCount>0)
        {
            yield return new WaitForEndOfFrame();
        }
        if(PostWaveDelay>=0)
        {
            yield return new WaitForSeconds(PostWaveDelay);
        }
        WavesCompleted?.Invoke();

    }

}
