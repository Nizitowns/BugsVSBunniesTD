using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<SpawnRequest> spawnRequests=new List<SpawnRequest>();

    public Action WaveStarted;
    public Action WaveEnded;

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
            for(int i=0;i<current.SpawnAmount;i++)
            {
                Instantiate(current.Prefab, transform.position, transform.rotation, transform);
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
    }

}
