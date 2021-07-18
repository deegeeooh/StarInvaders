using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    [SerializeField] float bullitLifeTime = 1.5f;
    [SerializeField] AudioClip lasersound;

    AudioSource myAudiosource;

    // Start is called before the first frame update
    void Start()
    {
        myAudiosource = GetComponent<AudioSource>();
        myAudiosource.PlayOneShot(lasersound);
        Destroy(gameObject,bullitLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnTriggerEnter2D(Collider2D collision)           laser is destroyed by DamageDealer.hit on collision
    //{
    //    Destroy(gameObject);
    //}

}
