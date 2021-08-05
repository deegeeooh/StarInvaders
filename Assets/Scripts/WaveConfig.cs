using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy Wave Config")]                                  // create new entry in the CreateAssetMenu (rightclick)
public class WaveConfig : ScriptableObject

{

    [SerializeField] GameObject enemyPrefab;                                        // enemy type
    [SerializeField] GameObject pathPrefab;                                         // path (Parent)
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float timeBeforeStarting = 1.5f;
    [SerializeField] int waypointToLoopFrom;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] int numberOfWaves;
    

    public GameObject GetEnemyPrefab() { return enemyPrefab; }
    public List<Transform> GetWayPoints() 
    {
        var waveWayPoints = new List<Transform>();

        foreach (Transform child in pathPrefab.transform)
        {
            // Debug.Log("child "+child);
            
            
            waveWayPoints.Add(child);
        }

        return waveWayPoints; 
    
    }
    public float GetTimeBetweenSpawns () { return timeBetweenSpawns; }
    public float GetspawnRandomFactor() { return spawnRandomFactor; }
    public int GetNumberOfEnemies() { return numberOfEnemies; }
    public float GetMoveSpeed() { return moveSpeed; }
    public int GetNumberOfLoops () { return numberOfWaves; }
    public int GetWayPointToLoopFrom () { return waypointToLoopFrom; }
    public float GetTimeBeforeStarting () { return timeBeforeStarting; }
    
    
   
   






}