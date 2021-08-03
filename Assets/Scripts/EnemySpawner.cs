using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] float timeBetweenWaves = 1f;
    [SerializeField] bool infiniteLoop = false;                                                     // all waves in this spawner will be repeated infinitely
    [SerializeField] bool singleLoop = false;                                                       // for a single repeated loop for a number of times
    [SerializeField] int numberOfSingleLoops = 1;
    int startingWave = 0;
    // WaveConfig waveconfig;  

    // Start is called before the first frame update

    


   public IEnumerator Start()
    
    {
        do
        {
            for (startingWave = 0; startingWave < waveConfigs.Count; startingWave++)
            {
                var currentWave = waveConfigs[startingWave];

                {

                    var nmberofLoops = waveConfigs[startingWave].GetNumberOfLoops();                 // number of loops from the current Wave
                    int loop = 0;
                    do
                    {
                        StartCoroutine(SpawnAllEnemiesInWave(currentWave));
                        yield return new WaitForSeconds(timeBetweenWaves);                            // time before next Wave Spawns 
                        Debug.Log("Starting Wave: " + startingWave);
                        loop++;
                    }
                    while (loop < nmberofLoops);



                    // will the current waves be looped infinitely?
                }

            }
            
        }
        while (infiniteLoop);
        Debug.Log("end level");
        Destroy(gameObject);                          // When the spawner is done, destroy the spawner so we can check if 
    }                                                 // the scene is completed in player.cs when no spawners are left

    // Update is called once per frame
    
    public IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        
            
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWayPoints()[0].transform.position,
                Quaternion.identity);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig,singleLoop,numberOfSingleLoops);
                
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
                               
        }
       
      
        
    }


}
