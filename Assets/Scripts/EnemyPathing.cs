using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] WaveConfig waveConfig;
    [SerializeField] float moveSpeed = 2f;
    int waypointIndex = 0;
    List<Transform> waypoints;



    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWayPoints();
        int a = waypoints.Count;
        Debug.Log("waypoints length  " + a);
        transform.position = waypoints[waypointIndex].transform.position;
    }
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = moveSpeed * Time.deltaTime;                                                 // using Time.deltaTime which is the interval time since the last frame update
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
            if (transform.position == targetPosition)                                                           // target reached? next waypoint
            {
                waypointIndex++;
            }

        }
        else
        {
            Destroy(gameObject);                                                                                // no more waypoints? destroy this gameObject
        }


    }


}
    // Update is called once per frame
    
