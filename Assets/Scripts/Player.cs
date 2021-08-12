using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [Header("Player Movement")]
    [SerializeField] float player_MovementSpeed = 8f;
    [SerializeField] float movementRestrictionX;                                                                        // movement restrictions for player ship
    [SerializeField] float movementRestrictionY_Top;
    [SerializeField] float movementRestrictionY_Bottom;
    [Header("Player stats")]
    [SerializeField] bool invulnerable = false;
    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 8f;                                                                       // how fast do projectiles travel
    [SerializeField] float projectileFiringPeriod =0.13f;                                                                 
    [SerializeField] int maxNumberOfBullitsOnScreen = 4;
    [SerializeField] bool autoFire = true;
    [SerializeField] float offsetLaserY = 0.3f;
    [SerializeField] float offsetLaserDouble_Left_X = -0.2f;
    [SerializeField] float offsetLaserDouble_Right_X = 0.2f;
    [SerializeField] float offsetLaserDouble_Y = 0.3f;
    [SerializeField] float offsetLaserTriple_Left_X = -0.4f;
    [SerializeField] float offsetLaserTriple_Right_X = 0.4f;
    [SerializeField] float offsetLaserTriple_Y = 0.1f;
    [SerializeField] bool singleLaser = true;
    [SerializeField] bool doubleLaser = false;
    [SerializeField] bool tripleLaser = false;
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
    int projectilesPerShot = 1;
    int extraBullitsOnScreen = 0;
    bool levelCleared;


    //cache references

    GameSession gameSession;
    GoldPot goldPot;

    private void Awake()
    {
        CheckSingleton();  // added a singleton so the player with attributes survives between levels

    }


    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        invulnerable = gameSession.IsPlayerInvulnarable();
        
        goldPot = FindObjectOfType<GoldPot>();
        healthRemaining = gameSession.GetHealthRemaining();                                 // introducing healtRemaining so we can reset to health;
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

    public void SetPlayerInvulnerable(bool isplayerinvulnarable)
    {
        invulnerable = isplayerinvulnarable;
    }



    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        if (Input.GetKeyDown(KeyCode.Alpha3 ))
        {
            tripleLaser = true;
            doubleLaser = false;
            singleLaser = false;
        }
        //CheckRemainingEnemies();
    }

    //private void CheckRemainingEnemies()                        // Did we kill everything in the current Scene?
    //{
    //    int numberofSpawners = FindObjectsOfType<EnemySpawner>().Length;
    //    int numberofEnemiesLeft = FindObjectsOfType<Enemy>().Length;
    //    //Debug.Log("Spawners active "+ numberofSpawners+"enemies left: " +numberofEnemiesLeft);

    //    if (numberofEnemiesLeft + numberofSpawners == 0 && !levelCleared)
    //    {
    //        levelCleared = true;
    //        FindObjectOfType<Levels>().LoadNextScene();
    //    }

    //}


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
        if (bullitcount < maxNumberOfBullitsOnScreen + extraBullitsOnScreen)
        {
            FirePrimaryLaser();
            gameSession.AddToNumberOfShots(projectilesPerShot);
        }

        yield return new WaitForSeconds(projectileFiringPeriod);
    }

    IEnumerator FireContinuously()
    {
        isFiring = true;

        while (Input.GetButton("Fire1"))
        {

            int bullitcount = FindObjectsOfType<laser>().Length;                                                        // how many laser are alive ? > dependant on bullitLifeTime = 1.5f; 
            if (bullitcount < maxNumberOfBullitsOnScreen + extraBullitsOnScreen)
            {
                FirePrimaryLaser();
                gameSession.AddToNumberOfShots(projectilesPerShot);

            }

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
        isFiring = false;
    }

    private void FirePrimaryLaser()
    {
        if (singleLaser)
        {
            GameObject laser = Instantiate(
                            laserPrefab,
                            new Vector3(transform.position.x, transform.position.y + offsetLaserY, 0),
                            Quaternion.identity) as GameObject;                                                              // IDK what the fuck Quaternion does, but here we instantiate the lasersprite prefab at the center of the player
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            // laser.transform.parent = this.transform;
        }
        else if (doubleLaser)
        {
            GameObject laserLeft = Instantiate(
                           laserPrefab,
                           new Vector3(transform.position.x + offsetLaserDouble_Left_X , transform.position.y + offsetLaserY, 0),
                           Quaternion.identity) as GameObject;                                                              // IDK what the fuck Quaternion does, but here we instantiate the lasersprite prefab at the center of the player
            laserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            
            GameObject laserRight = Instantiate(
                           laserPrefab,
                           new Vector3(transform.position.x + offsetLaserDouble_Right_X , transform.position.y + offsetLaserY, 0),
                           Quaternion.identity) as GameObject;                                                              // IDK what the fuck Quaternion does, but here we instantiate the lasersprite prefab at the center of the player
            laserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

        }


        else if (tripleLaser)
        {

            GameObject laser = Instantiate(
                            laserPrefab,
                            new Vector3(transform.position.x, transform.position.y + offsetLaserY, 0),
                            Quaternion.identity) as GameObject;                                                              // IDK what the fuck Quaternion does, but here we instantiate the lasersprite prefab at the center of the player
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

            GameObject laserLeft = Instantiate(
                           laserPrefab,
                           new Vector3(transform.position.x + offsetLaserTriple_Left_X , transform.position.y +offsetLaserTriple_Y , 0),
                           Quaternion.identity) as GameObject;                                                              // IDK what the fuck Quaternion does, but here we instantiate the lasersprite prefab at the center of the player
            laserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

            GameObject laserRight = Instantiate(
                           laserPrefab,
                           new Vector3(transform.position.x + offsetLaserTriple_Right_X , transform.position.y + offsetLaserTriple_Y , 0),
                           Quaternion.identity) as GameObject;                                                              // IDK what the fuck Quaternion does, but here we instantiate the lasersprite prefab at the center of the player
            laserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);




        }


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
        if (!damagedealer)
        {
            if (other.gameObject.GetComponent<Loot>() != null)
            {
                //Debug.Log("LOOT GEVANGEN");

                CheckCatchedLoot(other);

            }
        }                                             // protect agains null
        if (!invulnerable && damagedealer)
        {
            ProcessHit(damagedealer);
        }
        
    }

    private void CheckCatchedLoot(Collider2D other)
    {
        Loot loot = other.gameObject.GetComponent<Loot>();
        var value = loot.GetItemValue();
        if (loot.IsGold())
        { 
            gameSession.AddToGold(value);
            loot.PlaySound();
            loot.DestroyLootItem();
            
            if (value > 100)                    // spawn coin above GoldPot
            {
                goldPot.Add1000Coin();    
                
            }
            else
                
            {
                goldPot.Add100Coin();
            }

        }
        else if (loot.IsShooter_3way())
        {
            if (!tripleLaser)
            {
                singleLaser = false;
                doubleLaser = false;
                tripleLaser = true;
                projectilesPerShot = 3;
                extraBullitsOnScreen = 0;       // reset extra bullits when you get upgrade
                maxNumberOfBullitsOnScreen = maxNumberOfBullitsOnScreen * 3 + extraBullitsOnScreen * 3;
            }
        }
        else if (loot.IsShooter_Double())
        {
            if (!doubleLaser && !tripleLaser)
            {
                singleLaser = false;
                doubleLaser = true;
                tripleLaser = false;
                projectilesPerShot = 2;
                extraBullitsOnScreen = 0;
                maxNumberOfBullitsOnScreen = maxNumberOfBullitsOnScreen * 2 + extraBullitsOnScreen * 2;
            }
            
        }
        else if (loot.IsHealth())
        {
            if (healthRemaining < 800)
            {
                gameSession.AddToHealthRemaining(value);
                healthRemaining = gameSession.GetHealthRemaining();
            }

        }

        else if (loot.IsLife())
        {

        }
        else if (loot.IsBonusScore())
        {

        }
        else if (loot.IsPowerUp())
        {
            player_MovementSpeed += Mathf.Clamp(0.2f, 0, 13);
            projectileSpeed += Mathf.Clamp(0.2f, 0, 11);
            Mathf.Clamp(extraBullitsOnScreen++,0,2);
            // maxNumberOfBullitsOnScreen += extraBullitsOnScreen;
            //projectileFiringPeriod -= Mathf.Clamp(0.005f, 0.12f, 0.20f);
            //Debug.Log("Bullits on screen: " + maxNumberOfBullitsOnScreen + " proj speed " + projectileSpeed + " projectlifetime " + projectileFiringPeriod);

        }

        else if (loot.IsSpecial())
        {
            FindObjectOfType<RandomPot>().AddSpecialCoin();
        }


        loot.PlaySound();
        loot.DestroyLootItem();

    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        damageDealer.Hit();                     //destroy gameobject which dealth damage.
        //Debug.Log("Health remaining: " + healthRemaining );

        healthRemaining = healthRemaining-damageDealer.GetDamage();

        if (healthRemaining <= 0)
        {
            gameSession.SetHealthRemaining(0);
        }
        else
        {
            gameSession.AddToHealthRemaining(-damageDealer.GetDamage());
        }
        
                
        DamageHitLight(damageDealer.transform.position);
        
        if (healthRemaining <= 0)
        {
                healthRemaining = 0;
                DestructionHit(gameObject.transform.position);
                Destroy(gameObject,0.2f);
                FindObjectOfType<Levels>().LoadGameOverScene(); 
            
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


