using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [Header("Enemy stats")]
    [SerializeField] float health = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots =0.2f;
    [SerializeField] float maxTimeBetweenShots =3f;
    [SerializeField] LootTable lootTable;
    [Header("Projectile stats")]
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 8f;
    [SerializeField] float randomFactor = 1f;
    [SerializeField] bool shootAtPlayerPos = true;
    [Header("VFX")]
    [SerializeField] GameObject explosion_1_VFX;
    [SerializeField] float durationOfExplosion = 2f;
    [SerializeField] GameObject explosion_kill;
    [SerializeField] GameObject damage1_sprite_FacingUP;
    [SerializeField] GameObject damage2_sprite_FacingUP;
    [SerializeField] float durationOfdamage1 = 2f;
    [SerializeField] float durationOfdamage2 = 2f;
    [SerializeField] int maxDamageSpritesActive = 5;
    [Header("Sound settings")]
    [SerializeField] AudioClip soundHit;
    [SerializeField] AudioClip soundDestroyed;
    [SerializeField] AudioClip soundShoot;
    [Range(0f, 1f)] [SerializeField] float volumeHit = 1f;                      // cap the range
    [Range(0f, 1f)] [SerializeField] float volumeExplosion = 1f;

    Player player;
    AudioSource myAudiosource;
     

    // Start is called before the first frame updatea
    void Start()
    {
        // myAudiosource = GetComponent<AudioSource>();
        // myAudiosource.volume = volume;
        player = FindObjectOfType<Player>();
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            if (shootAtPlayerPos)
            {
                FireTowardsPlayer();
            }
            else
            {
                FireNormal();
            }
            
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
            AudioSource.PlayClipAtPoint(soundShoot, Camera.main.transform.position,volumeHit);
        }

    }

    private void FireNormal()
    {
        GameObject shot = Instantiate(
                    projectile,
                    transform.position,
                    Quaternion.identity) as GameObject;

            shot.GetComponent<Rigidbody2D>().velocity =
            new Vector2(Random.Range(0, randomFactor),
            -projectileSpeed - Random.Range(0, randomFactor));
            

    }

    private void FireTowardsPlayer()
    {

        GameObject shot = Instantiate(
                    projectile,
                    transform.position,
                    Quaternion.identity) as GameObject;


        if (FindObjectsOfType<Player>().Length == 0)                    
            {
                         shot.GetComponent<Rigidbody2D>().velocity =
                         new Vector2(Random.Range(0, randomFactor),
                         -projectileSpeed - Random.Range(0, randomFactor));
            return;

            }

        var playerPosX = player.transform.position.x;
        var playerposY = player.transform.position.y;
        var playerVectorX = transform.position.x - playerPosX;
        var playerVectorY = transform.position.y - playerposY;
        shot.GetComponent<Rigidbody2D>().velocity =
        
        
        new Vector2(-playerVectorX + Random.Range(-randomFactor,randomFactor),       // shoot at player position
        -playerVectorY + Random.Range(-randomFactor, randomFactor));

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return;  }                                             // protect agains null
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
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

            GameObject killExplosion = Instantiate(explosion_kill, transform.position, transform.rotation);
            Destroy(killExplosion,durationOfExplosion);
            
            AudioSource.PlayClipAtPoint(soundDestroyed, Camera.main.transform.position,volumeExplosion);
            // RollLoot();
            Destroy(gameObject);

            ///TODO: addtoScore maybe count totalkilled

            


        }
    }

    private void RollLoot()
    {
        var lootTableDropChance = lootTable.GetLootTableDropChance();
        List<ScriptableObject> lootItems = lootTable.GetLootItems();

        Debug.Log("LootItems "+ lootItems.Count);

        for (int startItem = 0; startItem < lootItems.Count; startItem++)
        {
            Debug.Log(lootItems[startItem].name);
        }

    





    }

}

