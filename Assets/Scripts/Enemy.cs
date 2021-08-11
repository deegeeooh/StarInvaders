using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [Header("Enemy stats")]
    [SerializeField] float health = 100;
    [SerializeField] int enemyScoreValue = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots =0.2f;
    [SerializeField] float maxTimeBetweenShots =3f;
    [SerializeField] LootTable lootTable;
    [Header("Projectile stats")]
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 8f;
    [SerializeField] int numberOfProjectiles = 1;
    [SerializeField] float randomFactor = 1f;
    [SerializeField] float spread = 0;
    [SerializeField] bool shootAtPlayerPos = false;
    [SerializeField] bool shootUP = false;
    [SerializeField] bool shootDown = false;
    [SerializeField] bool shootCircle = false;
    [Header("VFX")]
    [SerializeField] GameObject explosion_1_VFX;
    [SerializeField] float durationOfExplosion = 2f;
    [SerializeField] GameObject explosion_kill;
    [SerializeField] GameObject damage1_sprite_FacingUP;
    //[SerializeField] GameObject damage2_sprite_FacingUP;
    [SerializeField] float durationOfdamage1 = 2f;
    //[SerializeField] float durationOfdamage2 = 2f;
    [SerializeField] int maxDamageSpritesActive = 5;
    [Header("Sound settings")]
    [SerializeField] AudioClip soundHit;
    [SerializeField] AudioClip soundDestroyed;
    [SerializeField] AudioClip soundShoot;
    [Range(0f, 1f)] [SerializeField] float volumeHit = 1f;                      // cap the range
    [Range(0f, 1f)] [SerializeField] float volumeExplosion = 1f;

    Player player;
    AudioSource myAudiosource;
    GameSession gamesession;
    ScoreDisplay scoreDisplay;

    float positiveX, positiveY;
    float playerVectorX, playerVectorY;
    Vector2 normalisedVectorToPlayer;
    bool isDead = false;
    

    // Start is called before the first frame updatea
    void Start()
    {
        // myAudiosource = GetComponent<AudioSource>();
        // myAudiosource.volume = volume;
        player = FindObjectOfType<Player>();
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        gamesession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();                                    // TODO Sprite rotation
        
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            FireProjectile(numberOfProjectiles, shootUP, shootDown, shootCircle, shootAtPlayerPos);
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
            AudioSource.PlayClipAtPoint(soundShoot, Camera.main.transform.position,volumeHit);
        }

    }
            
    private void FireProjectile(int numberOfProjectiles, bool up, bool down, bool round, bool atPlayer)
    {

        int numberOfShotChoices = 0;

        if (up) { numberOfShotChoices++; }
        if (down) { numberOfShotChoices++; }
        if (round) { numberOfShotChoices++; }
        if (atPlayer) { numberOfShotChoices++; }

        if (numberOfShotChoices > 1)
        {
            var randomShotChoice = Random.Range(1, numberOfShotChoices);
        }
        



        for (int shotFired = 0; shotFired < numberOfProjectiles; shotFired++)
        {

            if (atPlayer && FindObjectsOfType<Player>().Length > 0)
            {
                normalisedVectorToPlayer = GetNormalisedVector2Player();
            }

            GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;

            if (round)
            {
                
                if (Random.Range(-1, 1) < 0) { positiveX = -1; } else { positiveX = 1; }        // gives me a 1 or -1
                if (Random.Range(-1, 1) < 0) { positiveY = -1; } else { positiveY = 1; }

                //Debug.Log("x , y "+ positiveX + positiveY);


                float xComponent = (positiveX + Random.Range(-spread, spread));
                float yComponent = (positiveY + Random.Range(-spread, spread));

                shot.GetComponent<Rigidbody2D>().velocity =

                //new Vector2((projectileSpeed + Random.Range(0, randomFactor)) * (positiveX + Random.Range(-spread, spread)),            // randomfactor for speed, spread for spread
                //((projectileSpeed + Random.Range(0, randomFactor)) * (positiveY + Random.Range(-spread, spread))));

                new Vector2(xComponent, yComponent).normalized * (projectileSpeed + Random.Range(0, randomFactor));
            }
            else if (up) 
            {
                if (atPlayer && FindObjectsOfType<Player>().Length > 0)
                {

                    shot.GetComponent<Rigidbody2D>().velocity = -normalisedVectorToPlayer * (projectileSpeed + Random.Range(0, randomFactor));

                }
                else
                {
                    float upX = Random.Range(-spread, spread);
                    float upY = projectileSpeed + Random.Range(0, randomFactor);

                    shot.GetComponent<Rigidbody2D>().velocity = new Vector2(upX, upY).normalized * (projectileSpeed + Random.Range(0, randomFactor)); 
                }
            }
            else if (down)
            {
                if (atPlayer && FindObjectsOfType<Player>().Length > 0)
                {
                    shot.GetComponent<Rigidbody2D>().velocity = -normalisedVectorToPlayer * (projectileSpeed+ Random.Range(0, randomFactor));
                }
                else
                {
                    float downX = Random.Range(-spread, spread);
                    float downY = -projectileSpeed + Random.Range(0, randomFactor);

                    shot.GetComponent<Rigidbody2D>().velocity = new Vector2(downX, downY).normalized * (projectileSpeed + Random.Range(0, randomFactor));
                }
            }
            
            Vector2 moveDirection = shot.GetComponent<Rigidbody2D>().velocity;              // Rotate projectile sprite in direction it is moving
            if (moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90;
                shot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

        }
    }

    private Vector2 GetNormalisedVector2Player()
    {
        var playerPosX = player.transform.position.x;
        var playerposY = player.transform.position.y;
        playerVectorX = transform.position.x - playerPosX+ Random.Range(-spread, spread);                   // determines the spread of the shot
        playerVectorY = transform.position.y - playerposY;// + Random.Range(-randomFactor, randomFactor);

        // playerVectorX, Y now have velocity relative to the distance between the two objects
        // that's why we use .normalised, to multiply with projectileSpeed

        Vector2 vectorToPlayer = new Vector2(playerVectorX, playerVectorY).normalized;
        return vectorToPlayer;

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return;  }                                             // protect agains null
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        gamesession.AddToNumberOfHits();
        damageDealer.Hit();                             //destroy gameobject which dealth damage.
        health -= damageDealer.GetDamage();
        Vector3 locationOther = damageDealer.transform.position;            // Get position of laserhit
        
        GameObject explosion = Instantiate(explosion_1_VFX, locationOther, transform.rotation);         //instantiate explosion there.
        Destroy(explosion, durationOfExplosion);
              
        GameObject damageSprite1 = Instantiate(damage1_sprite_FacingUP, locationOther, transform.rotation);
        damageSprite1.transform.parent = this.transform;                        // attach damage sprite transform to parent( this enemy)

        AudioSource.PlayClipAtPoint(soundHit, Camera.main.transform.position,volumeHit);

        
        // set COLOR of damagesprite to color of gameObject //

        //SpriteRenderer damagesprite = damageSprite1.GetComponent<SpriteRenderer>();
        //var kleur2 =  GetComponent<SpriteRenderer>().color;
        //damagesprite.color = kleur2;

        //Debug.Log(" "+kleur2);

        Destroy(damageSprite1, durationOfdamage1);

        
        
        if (health <= 0 && !isDead)                 // IsDead to ensure another bullithit can't enter this method
        {
            isDead = true;
            // transform.DetachChildren(); to detach children when object is a parent, children will stop moving though;
            GameObject killExplosion = Instantiate(explosion_kill, transform.position, transform.rotation);
            Destroy(killExplosion,durationOfExplosion);
            
            AudioSource.PlayClipAtPoint(soundDestroyed, Camera.main.transform.position,volumeExplosion);
            RollLoot(); 

            // Destroy(gameObject,0.1f);
            gamesession.AddToscore(enemyScoreValue);
            gamesession.AddToNumberOfKills();
            Destroy(gameObject);
        }
    }

    private void RollLoot()
    {
        

        List<LootItem> lootitems;
        lootitems = lootTable.GetLootItems();
        var lootTableDropChance = lootTable.GetLootTableDropChance();
        var numberOfLootitems = lootitems.Count;

        //LootItem item = FindObjectOfType <LootItem>();

        for (int startItem = 0; startItem < numberOfLootitems; startItem++)
        {
            GameObject itemGameObject = lootitems[startItem].GetLootItem();
            var weight = lootitems[startItem].GetItemWeight();
            float weigthedDropChance = (weight * lootTableDropChance);
            // Debug.Log("lootitem " + startItem + ": " + itemGameObject + " Chance: " + weigthedDropChance);

            float calculateDrop = Random.Range(0, 10001);
            if (calculateDrop <= weigthedDropChance)
            {
                // Debug.Log("calculateDrop  " + calculateDrop);
                GameObject dropItem = Instantiate(itemGameObject, transform.position, transform.rotation);
                dropItem.GetComponent<Rigidbody2D>().velocity = new Vector2(0 + Random.Range(-0.8f,0.8f) , -lootitems[startItem].GetItemdropSpeed()); // TODO: Serialize random Xvector for loot
                dropItem.transform.localScale = new Vector3(1.5f,1.5f ,1.5f);
                if (!lootTable.CanMultipleDropPerRoll())
                {
                    Debug.Log("Dropped Loot, exit For Loop");
                    break;  //exit for loop, only 1 drop per roll
                }
            }

        }
       // Destroy(gameObject);
        


    }

}

