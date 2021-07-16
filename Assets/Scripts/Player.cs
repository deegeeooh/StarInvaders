using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [SerializeField] float player_MovementSpeed = 11f;
    [SerializeField] float movementRestrictionX;                                                                        // movement restrictions for player ship
    [SerializeField] float movementRestrictionY_Top;
    [SerializeField] float movementRestrictionY_Bottom; 
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;                                                                       // how fast do projectiles travel
    [SerializeField] float projectileFiringPeriod = 2f;                                                                 
    [SerializeField] int maxNumberOfBullitsOnScreen = 4;
    [SerializeField] bool autoFire = true;

    bool isFiring;
    float xMin;
    float xMax;
    float yMin;
    float yMax;
        
    // Start is called before the first frame update
    void Start()
    {
        SetupMoveBoundaries();
    }

    


    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1") && !isFiring)                                                                   // Are we pressing a button and are we not firing already 
        {                                                                                                                // so the Coroutine is called simultaneously multiple times
            if (autoFire)
            {
                StartCoroutine(FireContinuously());
            }
            else
            {
                StartCoroutine(FireSingle());
            }




        }
                
    }

    IEnumerator FireSingle()
    {
        int bullitcount = FindObjectsOfType<laser>().Length;                                                        // how many laser are alive ? > dependant on bullitLifeTime = 1.5f; 
        if (bullitcount < maxNumberOfBullitsOnScreen)
        {
            FirePrimaryLaser();
        }

        yield return new WaitForSeconds(projectileFiringPeriod);
    }

    IEnumerator FireContinuously()
    {
        isFiring = true;

        while (Input.GetButton("Fire1"))
        {
            int bullitcount = FindObjectsOfType<laser>().Length;                                                        // how many laser are alive ? > dependant on bullitLifeTime = 1.5f; 
            if (bullitcount < maxNumberOfBullitsOnScreen)
            {
                FirePrimaryLaser();
            }

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
        isFiring = false;
    }

    private void FirePrimaryLaser()
    {
        GameObject laser = Instantiate(
                        laserPrefab,
                        transform.position,
                        Quaternion.identity) as GameObject;                                                                     // IDK what the fuck Quaternion does, but here we instantiate the lasersprite prefab at the center of the player
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
    }

    private void Move()
     {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * player_MovementSpeed;                                // using Time.deltatime to make movement FPS independent
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * player_MovementSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);                                            // Mathf.Clamp to limit range
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
               
        transform.position = new Vector2(newXPos, newYPos);                                                              // Update player movement every frame



    }
     private void SetupMoveBoundaries()
        {
            Camera gameCamera = Camera.main;
            xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + movementRestrictionX;                        // ViewportToWorldPoint has max coordinates 0.0 to 1.0
            xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + movementRestrictionX * -1;                   // Since we are interested in X, only the other axis can be left zero
            yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + movementRestrictionY_Bottom;                          
            yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - movementRestrictionY_Top;                            


        }

}

