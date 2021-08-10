using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] float timeBetweenWaves = 2f;
    [SerializeField] bool infiniteLoop = false;                                                     // all waves in this spawner will be repeated infinitely
    [SerializeField] bool singleLoop = false;                                                       // for a single repeated loop for a number of times
    [SerializeField] int numberOfSingleLoops = 1;
    [SerializeField] bool randomizeWaves = false;
   
    // initialise variables
    int startingWave = 0;
    // public int loop = 0;
    //WaveConfig waveconfig;

   public IEnumerator Start()
    
    {
        do
        {
            //var timeBeforeStarting = waveConfigs[0].GetTimeBeforeStarting();
            //yield return new WaitForSeconds(timeBeforeStarting);

            //for (startingWave = 0; startingWave < waveConfigs.Count; startingWave++)
            //{
            //    var currentWave = waveConfigs[startingWave];

            //    // Debug.Log("random waves " + randomizeWaves);
            //    //if (randomizeWaves)
            //    //{
            //    //    currentWave = waveConfigs[Random.Range(0, waveConfigs.Count-1)];
            //    //}

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
        //Debug.Log("end level");
        Destroy(gameObject);                          // When the spawner is done, destroy the spawner so we can check if 
    }                                                 // the scene is completed in player.cs when no spawners are left


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
        }
    }

    


    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            //Debug.Log("NumberofEnemies " + waveConfig.GetNumberOfEnemies());
            var newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWayPoints()[0].transform.position,
                Quaternion.identity);

            // Debug.Log("Enemy instantiated: " + enemyCount + " of: " + waveConfig.GetNumberOfEnemies());

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig,singleLoop,numberOfSingleLoops);
                
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
