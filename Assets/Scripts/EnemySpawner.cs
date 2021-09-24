using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] float timeBetweenWaves = 2f;
    [SerializeField] float timeBeforeStarting = 0;
    [SerializeField] bool infiniteLoop = false;                                                     // all waves in this spawner will be repeated infinitely
    [SerializeField] bool singleLoop = false;                                                       // for a single repeated loop for a number of times
    [SerializeField] int numberOfSingleLoops = 1;
    [Header("Wave Randomizer Settings")]
    [SerializeField] bool scaleWithLoops = false;
    [SerializeField] bool randomizeWaves = false;
    [SerializeField] bool rngTimeBetweenWaves = false;
    [SerializeField] float mintimeBetweenWaves = 0.5f;
    [SerializeField] bool rngNumberOfEnemies = false;
    [SerializeField] int minExtraEnemies = 1;
    [SerializeField] int maxnExtraEnemies = 1;

    // initialise variables
    int startingWave = 0;
    int numberOfEnemiesToSpawn;
    // public int loop = 0;
    GameSession gameSession;

   public IEnumerator Start()
    
    {
        gameSession = FindObjectOfType<GameSession>();
        gameSession.AddToSpawnersInLevel(1);
        gameSession.CheckIfLevelCompleted();
        do
        {
            yield return StartCoroutine(WaitForSeconds(timeBeforeStarting));                        //TODO: does this work currently?
            yield return StartCoroutine(SpawnAllWaves());

            //    //var nmberofLoops = waveConfigs[startingWave].GetNumberOfLoops();                 // number of loops from the current Wave
            //    //loop = 0;
            //    //do
            //    //{
            //        Debug.Log("startingwave " + startingWave + "waveconfig.count: " + waveConfigs.Count);
            //        //Debug.Log("numberofloops " + nmberofLoops + "loop: " + loop);
            //        yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            //        yield return new WaitForSeconds(timeBetweenWaves);                            // time before next Wave Spawns 
            //        // Debug.Log("Starting Wave: " + startingWave);
            //        //loop++;
            //    //}
            //    //while (loop < nmberofLoops);

            //    // will the current waves be looped infinitely?
            //
        }
        while (infiniteLoop);
        Debug.Log("end level");
        gameSession.AddToSpawnersInLevel(-1);
        gameSession.CheckIfLevelCompleted();
        Destroy(gameObject);
        
    }                                                 


    private IEnumerator SpawnAllWaves()     //TODO: add single waves, time between waves, time before start
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            
            var currentWave = waveConfigs[waveIndex];
            if (randomizeWaves)                                     // TODO: The same waves can spawn multiple times..
            {
                currentWave = waveConfigs[Random.Range(0, waveConfigs.Count)];
            }

            //Debug.Log("WaveConfigs.count: " + waveConfigs.Count + "waveindex " + waveIndex);
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            
            if (rngTimeBetweenWaves)
            {
                timeBetweenWaves = Random.Range(mintimeBetweenWaves, timeBetweenWaves);
            }

            yield return StartCoroutine(WaitForSeconds(timeBetweenWaves));
        }
    }

    


    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        numberOfEnemiesToSpawn = waveConfig.GetNumberOfEnemies();
        
        if (rngNumberOfEnemies)
        {
            numberOfEnemiesToSpawn += Random.Range(minExtraEnemies, maxnExtraEnemies + 1);
        }

        if (scaleWithLoops)
        {
            
            numberOfEnemiesToSpawn += gameSession.GetCurrentGameLoop()-1;
        }

        for (int enemyCount = 0; enemyCount < numberOfEnemiesToSpawn; enemyCount++)
        {
            //Debug.Log("NumberofEnemies " + waveConfig.GetNumberOfEnemies());
            var newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWayPoints()[0].transform.position,
                Quaternion.identity);

            // Debug.Log("Enemy instantiated: " + enemyCount + " of: " + waveConfig.GetNumberOfEnemies());

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig,singleLoop,numberOfSingleLoops);
            gameSession.AddToEnemiesSpawnedInLevel();                // Add 1 to enemies in level     
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }

    private IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        
    }

}
