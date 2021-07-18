using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots =0.2f;
    [SerializeField] float maxTimeBetweenShots =3f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 8f;
    [SerializeField] float randomFactor = 1f;
    [SerializeField] GameObject explosion_1_VFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] GameObject explosion_kill;


    Player player;
    
    // Start is called before the first frame update
    void Start()
    {
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
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }

    }

    private void Fire()
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
        
        

        shot.GetComponent<Rigidbody2D>().velocity = 
            new Vector2(playerPosX+Random.Range(0,randomFactor),
            -projectileSpeed - Random.Range(0,randomFactor));


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



        if (health <= 0)
        {

            GameObject killExplosion = Instantiate(explosion_kill, transform.position, transform.rotation);
            Destroy(killExplosion,durationOfExplosion);
            Destroy(gameObject);

            
        }
    }
}

