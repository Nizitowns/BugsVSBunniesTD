using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //The last EnemySpawner to start running, TODO: Replace with a more comprehensive system if multiple enemy spawners are used.
    public static EnemySpawner LatestLaunched;

    public List<EnemyWave> enemyWaves = new();

    private float enemiesWantedToSpawnThisWave;
    private float enemiesSpawnedThisWave;
    private float enemiesKilledThisWave;
    private float totalWavesCompleted;
    private float totalWavesToSpawn;
    public Action WaveEnded;
    public Action WavesCompleted;

    private List<Enemy> spawnedEnemies;

    [Header("Sound FX")] [SerializeField] private AudioClip waveIncomingSoundFx;
    
    public static event Action<EnemyWave.DifficultyRating> OnDiffucltyChanged;

    public Action WaveStarted;

    public void Start()
    {
        spawnedEnemies = new List<Enemy>();
        StartCoroutine(EnemySpawnerLoop());
    }

    //Log that an enemy for this wave has been killed
    public void LogKilledEnemy()
    {
        enemiesKilledThisWave++;
    }
    [Tooltip("On a scale from 0-1 returns how close the current wave is to completion.")]
    public float WavePercentage()
    {


        //What percent of enemies spawned this wave have we killed?
        return  Mathf.Min(Mathf.Max(0, enemiesKilledThisWave / Mathf.Max(1, enemiesWantedToSpawnThisWave)),
            1);

        /*Legacy Code
         * 
        if (enemyWaves.Count <= 0)
            //If there are no spawn requests we assume 100% completion of  this wave. (Last wave ends and we have no requests left)
            return 1;
         *         //What percent of enemies spawned this wave have we killed?
        return 1 - Mathf.Min(Mathf.Max(0, transform.childCount / Mathf.Max(1, enemiesSpawnedThisWave)),
            1);
         */
    }

    [Tooltip("On a scale from 0-1 returns how close this entire enemy spawner is to 100% completion.")]
    public float TotalPercentage()
    {
        return totalWavesCompleted / totalWavesToSpawn;
    }

    public EnemyWave.DifficultyRating CurrentWaveDifficulty()
    {
        if (enemyWaves.Count <= 0)
            //No waves? This is easy mode!
            return EnemyWave.DifficultyRating.Easy;

        var current = enemyWaves[0];
        return current.ReportedDifficultyRating;
    }

    private bool IsEnemiesDead()
    {
        foreach (var enemy in spawnedEnemies)
            if (!enemy.IsDead) return false;

        return true;
        return transform.childCount <= 0;
    }

    // Spawn Loop
    public IEnumerator EnemySpawnerLoop()
    {
        bool lastWasCompletionBased = true;

        while (enemyWaves.Count > 0)
        {
            LatestLaunched = this;
            if (enemyWaves.Count > totalWavesToSpawn) totalWavesToSpawn = enemyWaves.Count;
            WaveStarted?.Invoke();
            var current = enemyWaves[0];
            OnDiffucltyChanged?.Invoke(current.ReportedDifficultyRating);
            if (lastWasCompletionBased)
            {
                enemiesKilledThisWave = 0;
                enemiesSpawnedThisWave = 0;
                enemiesWantedToSpawnThisWave = 0;
                for (int i = 0; i < enemyWaves.Count; i++)
                {
                    var currentWave = enemyWaves[i];
                    foreach (var enemyToSpawn in currentWave.enemiesToSpawn)
                        enemiesWantedToSpawnThisWave += enemyToSpawn.SpawnAmount;  //TODO: enemiesToSpawn[0]

                    if (currentWave.WaitUntilCompletion)
                        break;
                }
                lastWasCompletionBased = false;
            }
            if(current.WaitUntilCompletion)
            {
                lastWasCompletionBased = true;
            }

            spawnedEnemies = new List<Enemy>();
            SoundFXPlayer.PlaySFX(SoundFXPlayer.Source, waveIncomingSoundFx);
            foreach (var enemyToSpawn in current.enemiesToSpawn)
            {
                for (var i = 0; i < enemyToSpawn.SpawnAmount; i++)   //TODO: enemiesToSpawn[0]
                {
                    enemiesSpawnedThisWave++;
                    var spawnPos = transform.position;
                    PathNode initTarget = null;
                    if(enemyToSpawn.EnemyConfig == null)   //TODO: enemiesToSpawn[0]
                    {
                        Debug.LogError("Enemy Configuration is null, enemy configs are either unset or may have been lost in data migration?");
                    }

                    if (enemyToSpawn.snapSpawning) initTarget = PathManager.Instance.getEntryNode(); //TODO: enemiesToSpawn[0]
                    if (initTarget != null) spawnPos = initTarget.transform.position;
                    var g = Instantiate(enemyToSpawn.EnemyConfig.prefab, spawnPos, transform.rotation, transform);   //TODO: enemiesToSpawn[0]
                    var enemy = g.GetComponent<Enemy>();
                    spawnedEnemies.Add(enemy);
                    enemy.Initialize(enemyToSpawn.EnemyConfig);  //TODO: enemiesToSpawn[0]

                    if (initTarget != null)
                    {
                        var p = g.GetComponent<Pathfinder>();
                        p.currentNode = initTarget;
                    }

                    yield return new WaitForSeconds(enemyToSpawn.spawnInterval);    //TODO: enemiesToSpawn[0]
                }
            }

            if (current.WaitUntilCompletion) yield return new WaitUntil(IsEnemiesDead);
            totalWavesCompleted++;
            WaveEnded?.Invoke();

            yield return new WaitForSeconds(current.PostWaveDelay);   //TODO: enemiesToSpawn[0]

            yield return new WaitForEndOfFrame();
            enemyWaves.RemoveAt(0);
        }

        while (transform.childCount > 0) yield return new WaitForEndOfFrame();
        // if (PostWaveDelay >= 0) yield return new WaitForSeconds(PostWaveDelay);  //BUG: Not sure what this was meant for, there was a post wave delay for the level spawner script itself.
        if (LatestLaunched == this)
            LatestLaunched = null;
        WavesCompleted?.Invoke();
    }

    [Serializable]
    public class EnemyWave : ISerializationCallbackReceiver
    {
        [Tooltip("Optional rating of difficulty for any current wave UI.")]
        public DifficultyRating ReportedDifficultyRating;

        public enum DifficultyRating
        {
            Normal,
            Easy,
            Boss
        }

        [Tooltip("List of enemies for this spawn request.")]
        public List<EnemyToSpawn> enemiesToSpawn;    // List of EnemyToSpawn inside EnemyWave

        [Tooltip("How long to wait before next wave.")]
        public float PostWaveDelay = 6.0f;

        [Tooltip("True if the next wave must wait before this one is finished to start.")]
        public bool WaitUntilCompletion = true;

        //Set Defaults
        public EnemyWave()
        {
            enemiesToSpawn = new List<EnemyToSpawn>();
        }

        // This method is called after Unity deserializes your object.
        public void OnAfterDeserialize()    //TODO: Can't set WaitUntilCompletion to false with this method. Just needing to set defaults this is a temp fix.
        {
            if (PostWaveDelay == 0.0f) PostWaveDelay = 6.0f;

            if (WaitUntilCompletion == false) WaitUntilCompletion = true;
        }

        // Required by the interface but not used.
        public void OnBeforeSerialize() { }
    }
    
    [Serializable]
    public class EnemyToSpawn
    {
        public EnemyScriptableObject EnemyConfig;

        [Tooltip("How many Enemies to spawn with this order.")]
        public float SpawnAmount;

        [Tooltip("Delay between each spawning.")]
        public float spawnInterval;

        [Tooltip("Should this enemy snap to its entry node when it spawns?")]
        public bool snapSpawning = true;
    }
}