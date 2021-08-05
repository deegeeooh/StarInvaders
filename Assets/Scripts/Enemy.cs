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
    [SerializeField] bool shootThreeWay = false;
    [SerializeField] bool shootThreeWayUp = false;
    [SerializeField] bool shootThreeWayDown = false;
    [SerializeField] bool shootThreeWayRound = false;
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
        //Vector3 moveDirection = gameObject.transform.position;           

        //if (moveDirection != Vector3.left && moveDirection != Vector3.right)
        //{
        //    float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + -90 ;
        //    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //}
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            //if (shootAtPlayerPos)
            //{
            //    FireTowardsPlayer();
            //}
            if (shootThreeWay)
            {
                FireMultiple(numberOfProjectiles, shootThreeWayUp, shootThreeWayDown, shootThreeWayRound, shootAtPlayerPos);
            }
            else
            {
                FireNormal();
            }
            
            
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
            AudioSource.PlayClipAtPoint(soundShoot, Camera.main.transform.position,volumeHit);
        }

    }
    private Vector2 GetNormalisedVector2Player()
    {
        var playerPosX = player.transform.position.x;
        var playerposY = player.transform.position.y;
        playerVectorX = transform.position.x - playerPosX + Random.Range(-randomFactor, randomFactor);
        playerVectorY = transform.position.y - playerposY + Random.Range(-randomFactor, randomFactor);

        // playerVectorX, Y now have velocity relative to the distance between the two objects
        // that's why we use .normalised, to multiply with projectileSpeed

        Vector2 vectorToPlayer = new Vector2(playerVectorX, playerVectorY).normalized;
        return vectorToPlayer;

    }


    private void GetPlayerVector(out float playerVectorX, out float playerVectorY) // Old routine 
    {
        var playerPosX = player.transform.position.x;
        var playerposY = player.transform.position.y;
        playerVectorX = transform.position.x - playerPosX;
        playerVectorY = transform.position.y - playerposY;
    }

    private void FireNormal()
    {
        GameObject shot = Instantiate(
                    projectile,
                    transform.position,
                    Quaternion.identity) as GameObject;

        shot.GetComponent<Rigidbody2D>().velocity =                             // shoot down with random.range
        new Vector2(Random.Range(0, randomFactor),
        -projectileSpeed - Random.Range(0, randomFactor));

    }

    private void FireTowardsPlayer()
    {

        GameObject shot = Instantiate(
                    projectile,
                    transform.position,
                    Quaternion.identity) as GameObject;


        if (FindObjectsOfType<Player>().Length == 0)                    // player dead?
        {
            shot.GetComponent<Rigidbody2D>().velocity =
            new Vector2(Random.Range(0, randomFactor),
            -projectileSpeed - Random.Range(0, randomFactor));
            return;

        }
        
        
        // GetPlayerVector(out playerVectorX, out playerVectorY);   This will get the unnormalized vectors

        normalisedVectorToPlayer = GetNormalisedVector2Player();
        
        var distance = Vector2.Distance(player.transform.position, transform.position);
        Debug.Log("playervectorx: " + playerVectorX + "playerVectorY: " + playerVectorY+"distance "+distance+" Normalisedvector " + normalisedVectorToPlayer);



        shot.GetComponent<Rigidbody2D>().velocity = -normalisedVectorToPlayer * projectileSpeed;
            
        
        //new Vector2(-playerVectorX + Random.Range(-randomFactor, randomFactor),       // shoot at player position
        //-playerVectorY + Random.Range(-randomFactor, randomFactor));

    }

    private void FireMultiple(int numberOfProjectiles, bool up, bool down, bool round, bool atPlayer)
    {
        

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

                Debug.Log("x , y "+ positiveX + positiveY);


                shot.GetComponent<Rigidbody2D>().velocity =

                new Vector2(projectileSpeed * (positiveX + Random.Range(-randomFactor, randomFactor)),
                (projectileSpeed * (positiveY + Random.Range(-randomFactor, randomFactor))));
            }
            else if (up)
            {
                shot.GetComponent<Rigidbody2D>().velocity =

                new Vector2(projectileSpeed * Random.Range(-spread, spread),
                projectileSpeed + Random.Range(-randomFactor, randomFactor));
            }
            else if (down)
            {
                if (atPlayer && FindObjectsOfType<Player>().Length > 0)
                {
                    shot.GetComponent<Rigidbody2D>().velocity = -normalisedVectorToPlayer *
                        (projectileSpeed + Random.Range(-randomFactor, randomFactor));
                }
                else
                {
                    shot.GetComponent<Rigidbody2D>().velocity = 

                    new Vector2(projectileSpeed * Random.Range(-randomFactor, randomFactor),
                    -projectileSpeed - Random.Range(-randomFactor, randomFactor));
                }
            }


        }
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
        damageSprite1.transform.parent = this.transform;                        // attach damage sprite to parent( this enemy)

        AudioSource.PlayClipAtPoint(soundHit, Camera.main.transform.position,volumeHit);

        
        // set COLOR of damagesprite to color of gameObject //

        //SpriteRenderer damagesprite = damageSprite1.GetComponent<SpriteRenderer>();
        //var kleur2 =  GetComponent<SpriteRenderer>().color;
        //damagesprite.color = kleur2;

        //Debug.Log(" "+kleur2);

        Destroy(damageSprite1, durationOfdamage1);

        
        
        if (health <= 0)
        {

            // transform.DetachChildren(); to detach children when object is a parent, children will stop moving though;
            GameObject killExplosion = Instantiate(explosion_kill, transform.position, transform.rotation);
            Destroy(killExplosion,durationOfExplosion);
            
            AudioSource.PlayClipAtPoint(soundDestroyed, Camera.main.transform.position,volumeExplosion);
            RollLoot(); 

            // Destroy(gameObject,0.1f);
            gamesession.AddToscore(enemyScoreValue);
            gamesession.AddToNumberOfKills();

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
                dropItem.GetComponent<Rigidbody2D>().velocity = new Vector2(0,-lootitems[startItem].GetItemdropSpeed());
            }

        }
        Destroy(gameObject, 0.1f);
        


    }

}

