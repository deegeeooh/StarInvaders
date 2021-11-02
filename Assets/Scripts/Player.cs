using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [Header("Player Movement")]
    [SerializeField] private float player_MovementSpeed = 8f;

    [SerializeField] private float movementRestrictionX;                                                                        // movement restrictions for player ship
    [SerializeField] private float movementRestrictionY_Top;
    [SerializeField] private float movementRestrictionY_Bottom;

    [Header("Player stats")]
    [SerializeField] private bool invulnerable = false;

    [Header("Projectile")]
    [SerializeField] private GameObject laserPrefab;

    [SerializeField] private float projectileSpeed = 8f;                                                                       // how fast do projectiles travel
    [SerializeField] private float projectileFiringPeriod = 0.13f;
    [SerializeField] private int maxNumberOfBullitsOnScreen = 4;
    [SerializeField] private int laserDamage = 100;
    [SerializeField] private int laserDamageIncrease = 20;
    [SerializeField] private bool autoFire = true;
    [SerializeField] private float offsetLaserY = 0.3f;
    [SerializeField] private float offsetLaserDouble_Left_X = -0.2f;
    [SerializeField] private float offsetLaserDouble_Right_X = 0.2f;
    [SerializeField] private float offsetLaserDouble_Y = 0.3f;
    [SerializeField] private float offsetLaserTriple_Left_X = -0.4f;
    [SerializeField] private float offsetLaserTriple_Right_X = 0.4f;
    [SerializeField] private float offsetLaserTriple_Y = 0.1f;
    [SerializeField] private bool singleLaser = true;
    [SerializeField] private bool doubleLaser = false;
    [SerializeField] private bool tripleLaser = false;

    [Header("DamageSprites")]
    [SerializeField] private GameObject damage1_sprite_FacingDOWN;
    [SerializeField] private GameObject damage2_sprite_FacingDOWN;
    [SerializeField] private GameObject destructionSprite;
    [SerializeField] private float durationOfdamage1 = 2f;
    [SerializeField] private float durationOfdamage2 = 2f;
    [SerializeField] private int maxDamageSpritesActive = 5;

    [Header("DamageSounds")]
    [SerializeField] private AudioClip sound_Damage;

    [Range(0f, 1f)] [SerializeField] private float volume_sound_Damage = 1f;
    [SerializeField] private AudioClip sound_Destruction;
    [Range(0f, 1f)] [SerializeField] private float volume_sound_Destruction = 1f;

    //state variables

    private bool isFiring; // standard = false;
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;
    private int healthRemaining;                                        // you initialise here but can't assign, that's in a method
    private int projectilesPerShot = 1;
    private int extraBullitsOnScreen = 0;
    private bool levelCleared;
    private int numberOfActiveLasers = 1;

    //cache references

    private GameSession gameSession;
    private GoldPot goldPot;

    private void Awake()
    {
        CheckSingleton();  // added a singleton so the player with attributes survives between levels
    }

    // Start is called before the first frame update
    private void Start()
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
    private void Update()
    {
        Move();
        Fire();
        if (Input.GetKeyDown(KeyCode.Alpha3))
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

    private IEnumerator FireSingle()
    {
        int bullitcount = FindObjectsOfType<laser>().Length;                                                            // how many laser are alive ? > dependant on bullitLifeTime = 1.5f;
        if (bullitcount < maxNumberOfBullitsOnScreen + extraBullitsOnScreen)
        {
            FirePrimaryLaser();
            gameSession.AddToNumberOfShots(projectilesPerShot);
        }

        yield return new WaitForSeconds(projectileFiringPeriod);
    }

    private IEnumerator FireContinuously()
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
            InitialiseLaser(laser);
            // laser.transform.parent = this.transform;


        }
        else if (doubleLaser)
        {
            GameObject laserLeft = Instantiate(
                           laserPrefab,
                           new Vector3(transform.position.x + offsetLaserDouble_Left_X, transform.position.y + offsetLaserY, 0),
                           Quaternion.identity) as GameObject;                                                              // IDK what the fuck Quaternion does, but here we instantiate the lasersprite prefab at the center of the player
            laserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            InitialiseLaser(laserLeft);
            GameObject laserRight = Instantiate(
                           laserPrefab,
                           new Vector3(transform.position.x + offsetLaserDouble_Right_X, transform.position.y + offsetLaserY, 0),
                           Quaternion.identity) as GameObject;                                                              // IDK what the fuck Quaternion does, but here we instantiate the lasersprite prefab at the center of the player
            laserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            InitialiseLaser(laserRight);
        }
        else if (tripleLaser)
        {
            GameObject laser = Instantiate(
                            laserPrefab,
                            new Vector3(transform.position.x, transform.position.y + offsetLaserY, 0),
                            Quaternion.identity) as GameObject;                                                              // IDK what the fuck Quaternion does, but here we instantiate the lasersprite prefab at the center of the player
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            InitialiseLaser(laser);
            GameObject laserLeft = Instantiate(
                           laserPrefab,
                           new Vector3(transform.position.x + offsetLaserTriple_Left_X, transform.position.y + offsetLaserTriple_Y, 0),
                           Quaternion.identity) as GameObject;                                                              // IDK what the fuck Quaternion does, but here we instantiate the lasersprite prefab at the center of the player
            laserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            InitialiseLaser(laserLeft);
            GameObject laserRight = Instantiate(
                           laserPrefab,
                           new Vector3(transform.position.x + offsetLaserTriple_Right_X, transform.position.y + offsetLaserTriple_Y, 0),
                           Quaternion.identity) as GameObject;                                                              // IDK what the fuck Quaternion does, but here we instantiate the lasersprite prefab at the center of the player
            laserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            InitialiseLaser(laserRight);
        }
    }
    
    private void InitialiseLaser(GameObject laser)
    {
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        laser.GetComponent<DamageDealer>().SetDamage(laserDamage);
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
            gameSession.AddToGold(value);           // for bonus
            gameSession.AddToscore(value);          // for score
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
                maxNumberOfBullitsOnScreen = maxNumberOfBullitsOnScreen * 3;
            }
            else                              // points reward when caught again
            {
                gameSession.AddToscore(value);
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
                maxNumberOfBullitsOnScreen = maxNumberOfBullitsOnScreen * 2;
            }
            else
            {
                gameSession.AddToscore(value);
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
            Mathf.Clamp((player_MovementSpeed = player_MovementSpeed + 0.2f), 0, 14); 
            Mathf.Clamp((projectileSpeed = projectileSpeed + 0.2f), 0, 11);
            Mathf.Clamp(extraBullitsOnScreen += numberOfActiveLasers, 0, 3 * numberOfActiveLasers);
            Mathf.Clamp((projectileFiringPeriod = projectileFiringPeriod - 0.005f), 0, 0.08f);
            Mathf.Clamp(laserDamage += laserDamageIncrease,0,300);
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

        healthRemaining = healthRemaining - damageDealer.GetDamage();

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
            Destroy(gameObject, 0.2f);
            FindObjectOfType<Levels>().LoadGameOverScene();
        }
    }

    private void DestructionHit(Vector3 hitlocation)
    {
        GameObject destructionSprite1 = Instantiate(destructionSprite, hitlocation, Quaternion.identity);
        AudioSource.PlayClipAtPoint(sound_Destruction, Camera.main.transform.position, volume_sound_Destruction);
        Destroy(destructionSprite1, 1.1f);
    }

    private void DamageHitLight(Vector3 hitLocation)
    {
        GameObject damageSprite1 = Instantiate(damage1_sprite_FacingDOWN, hitLocation, Quaternion.identity);
        damageSprite1.transform.parent = gameObject.transform;                    // attach damage sprite to parent( this player object)
        AudioSource.PlayClipAtPoint(sound_Damage, Camera.main.transform.position, volume_sound_Damage);
    }
}