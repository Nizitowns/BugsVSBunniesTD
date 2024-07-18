using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<SpawnRequest> spawnRequests=new List<SpawnRequest>();

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
    }


    public void Start()
    {
        StartCoroutine(EnemySpawnerLoop());
    }

    public IEnumerator EnemySpawnerLoop()
    {
        while (spawnRequests.Count > 0)
        {
            SpawnRequest current = spawnRequests[0];
            for(int i=0;i<current.SpawnAmount;i++)
            {
                Instantiate(current.Prefab, transform.position, transform.rotation, transform);
                yield return new WaitForSeconds(current.SpawnDelayTime);
            }

            yield return new WaitForSeconds(current.PostWaveDelay);

            yield return new WaitForEndOfFrame();
            spawnRequests.RemoveAt(0);
        }
    }
}
