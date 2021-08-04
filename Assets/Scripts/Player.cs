using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [Header("Player Movement")]
    [SerializeField] float player_MovementSpeed = 11f;
    [SerializeField] float movementRestrictionX;                                                                        // movement restrictions for player ship
    [SerializeField] float movementRestrictionY_Top;
    [SerializeField] float movementRestrictionY_Bottom;
    [Header("Player stats")]
    [SerializeField] int health = 200;
    [SerializeField] int lifes = 3;
    [SerializeField] bool invulnerable = false;
    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;                                                                       // how fast do projectiles travel
    [SerializeField] float projectileFiringPeriod = 2f;                                                                 
    [SerializeField] int maxNumberOfBullitsOnScreen = 4;
    [SerializeField] bool autoFire = true;
    [SerializeField] float offsetLaser = 0.3f;
    [Header("DamageSprites")]
    [SerializeField] GameObject damage1_sprite_FacingDOWN;
    [SerializeField] GameObject damage2_sprite_FacingDOWN;
    [SerializeField] GameObject destructionSprite;
    [SerializeField] float durationOfdamage1 = 2f;
    [SerializeField] float durationOfdamage2 = 2f;
    [SerializeField] int maxDamageSpritesActive = 5;
    [Header("DamageSounds")]
    [SerializeField] AudioClip sound_Damage;
    [Range(0f, 1f)] [SerializeField] float volume_sound_Damage= 1f;
    [SerializeField] AudioClip sound_Destruction;
    [Range(0f, 1f)] [SerializeField] float volume_sound_Destruction = 1f;

    //state variables

    bool isFiring; // standard = false;
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    int healthRemaining;                                        // you initialise here but can't assign, that's in a method 

    //cache references

    GameSession gameSession;



    // Start is called before the first frame update
    void Start()
    {

        CheckSingleton();                                           // added a singleton so the player with attributes survives between levels
        gameSession = FindObjectOfType<GameSession>();
        healthRemaining = health;                                   // introducing healtRemaining so we can reset to health;
        SetupMoveBoundaries();                                      // when dying
        
    }
    
    private void CheckSingleton()
    {
        if (FindObjectsOfType<Player>().Length > 1)
        {
                     
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
                       
            
            DontDestroyOnLoad(gameObject);
           
                
        }

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        CheckRemainingEnemies();
    }

    
    private void CheckRemainingEnemies()                        // Did we kill everything in the current Scene?
    {
        int numberofSpawners = FindObjectsOfType<EnemySpawner>().Length;            
        int numberofEnemiesLeft = FindObjectsOfType<Enemy>().Length;
        // Debug.Log("Spawners active "+ numberofSpawners+"enemies left: " +numberofEnemiesLeft);
        if (numberofEnemiesLeft + numberofSpawners == 0)
        {
            FindObjectOfType<Levels>().LoadNextScene();     
            

        }
        

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
                StartCoroutine(FireSingle());                                                                           //  Stopcoroutine (routinename) to stop specific
            }

        }
                
    }

    IEnumerator FireSingle()
    {
        int bullitcount = FindObjectsOfType<laser>().Length;                                                            // how many laser are alive ? > dependant on bullitLifeTime = 1.5f; 
        if (bullitcount < maxNumberOfBullitsOnScreen)
        {
            FirePrimaryLaser();
            gameSession.AddToNumberOfShots();
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
                gameSession.AddToNumberOfShots();

            }

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
        isFiring = false;
    }

    private void FirePrimaryLaser()
    {
        GameObject laser = Instantiate(
                        laserPrefab,
                        new Vector3(transform.position.x,transform.position.y+offsetLaser,0),
                        Quaternion.identity) as GameObject;                                                              // IDK what the fuck Quaternion does, but here we instantiate the lasersprite prefab at the center of the player
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

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        DamageDealer damagedealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damagedealer) { return; }                                             // protect agains null
        if (!invulnerable)
        {
            ProcessHit(damagedealer);
        }
        
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        damageDealer.Hit();                     //destroy gameobject which dealth damage.
        Debug.Log("Health remaining: " + healthRemaining + " Lifes remaining: " + lifes);
        
        healthRemaining -= damageDealer.GetDamage();    
        //Vector3 locationOther = damageDealer.transform.position;
        DamageHitLight(damageDealer.transform.position);
        if (healthRemaining <= 0)
        {
            lifes = lifes - 1;
            FindObjectOfType<GameSession>().UpdateLivesRemaining(-1);
            healthRemaining = health;

            
            if (lifes == 0)                                 // TODO: this needs to go elsewhere so we can destroy player and
            {                                               // re instantiate the player instead of destroy the gameObject here
                DestructionHit(gameObject.transform.position);
                Destroy(gameObject);
                FindObjectOfType<Levels>().LoadGameOverScene();
            }
        }
    }

    private void DestructionHit(Vector3 hitlocation)
    {
        GameObject destructionSprite1 = Instantiate(destructionSprite , hitlocation, Quaternion.identity);
        AudioSource.PlayClipAtPoint(sound_Destruction, Camera.main.transform.position,volume_sound_Destruction);
        Destroy(destructionSprite1, 1.1f);
    }


    private void DamageHitLight(Vector3 hitLocation)
    {
       
        GameObject damageSprite1 = Instantiate(damage1_sprite_FacingDOWN , hitLocation, Quaternion.identity);
        damageSprite1.transform.parent = gameObject.transform;                    // attach damage sprite to parent( this player object)
        AudioSource.PlayClipAtPoint(sound_Damage, Camera.main.transform.position,volume_sound_Damage );
    }
}


