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
    public IEnumerator EnemySpawnerLoop()
    {
        while (spawnRequests.Count > 0)
        {

            WaveStarted?.Invoke();
            SpawnRequest current = spawnRequests[0];
            for (int i = 0; i < current.SpawnAmount; i++)
            {

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
            WaveEnded?.Invoke();

            yield return new WaitForSeconds(current.PostWaveDelay);


            yield return new WaitForEndOfFrame();
            spawnRequests.RemoveAt(0);
        }

        while(transform.childCount>0)
        {
            yield return new WaitForEndOfFrame();
        }
        WavesCompleted?.Invoke();

    }

}
