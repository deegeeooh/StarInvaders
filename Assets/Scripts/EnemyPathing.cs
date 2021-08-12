using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    GameSession gameSession;
    int waypointIndex = 0;
    List<Transform> waypoints;

    int loopsCompleted = 0;
    bool singleLoop;
    int numberOfLoops;
    // gameLoop variables
    float gameLoopMovementspeedFactor = 0;


    // Player player;                                                       *** for player position


    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        gameLoopMovementspeedFactor = gameSession.GetLoopMovementSpeedFactor();          // gameLoop speedfactor


        // player = FindObjectOfType<Player>();                               *** for player position
        waypoints = waveConfig.GetWayPoints();
        // int a = waypoints.Count;
        //Debug.Log("waypoints length  " + a);
        transform.position = waypoints[waypointIndex].transform.position;
    }
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig, bool single, int loops)

    {
        numberOfLoops = loops;
        singleLoop = single;
        this.waveConfig = waveConfig;
    }


    private void Move()
    
    {
        //Debug.Log("here: "+ waypointIndex+" "+ waypoints.Count);
        if (waypointIndex <= waypoints.Count -1)
        {
            /*  var playerPosX = player.transform.position.x;                     *** for player position
              var targetPosition = new Vector3(playerPosX, waypoints[waypointIndex].transform.position.y, 0);*/

            //Vector3 targetPosition = MoveToNextWaypoint();
            
            var targetPosition = waypoints[waypointIndex].transform.position;   //TODO add randomization to targetposition.
            var movementThisFrame = (waveConfig.GetMoveSpeed()+gameLoopMovementspeedFactor) * Time.deltaTime;                                                 // using Time.deltaTime which is the interval time since the last frame update
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
           

            if (transform.position == targetPosition)                                                           // target reached? next waypoint
            {
                waypointIndex++;
                //Debug.Log("waypointIndex " + waypointIndex);
            }
            //if (waypointIndex == waypoints.Count && singleLoop)
            //{
            //    loopsCompleted++;
            //    if (loopsCompleted < numberOfLoops)
            //    {
            //        waypointIndex = waveConfig.GetWayPointToLoopFrom();
            //        // Debug.Log("completed: " + loopsCompleted + "numberofLoops: " + numberOfLoops + "newWP: " + waypointIndex);
                    
            //    }
            //    else
            //    {
            //        //loopsCompleted = 0;
            //        Destroy(gameObject);
            //        FindObjectOfType<GameSession>().AddToNumberOfMissed();  // escaped enemies
                    
            //    }
            //}


        }
        else //if (!singleLoop)
        {
            FindObjectOfType<GameSession>().AddToNumberOfEscaped();          // escaped enemies
            gameSession.AddToEnemiesKilledInLevel();
            gameSession.CheckIfLevelCompleted();
            Destroy(gameObject);                                                                                // no more waypoints? destroy this gameObject
                        //Debug.Log("Destroyed ");
        }

    }

    //private Vector3 MoveToNextWaypoint()
    //{
    //    var targetPosition = waypoints[waypointIndex].transform.position;   //TODO add randomization to targetposition.
    //    var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;                                                 // using Time.deltaTime which is the interval time since the last frame update
    //    transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
    //    return targetPosition;
    //}




}
 