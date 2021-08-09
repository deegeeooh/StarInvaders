using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    [SerializeField] float bullitLifeTime = 1.5f;
    [SerializeField] AudioClip lasersound;
    [Range(0f, 1f)] [SerializeField]  float volume=1f;

    AudioSource myAudiosource;

    // Start is called before the first frame update
    private void Awake()
    {
        if (FindObjectsOfType<laser>().Length > 1)
        {
            DontDestroyOnLoad(gameObject);
        }
    }


    void Start()
    {
        myAudiosource = GetComponent<AudioSource>();
        myAudiosource.volume = volume;
        myAudiosource.PlayOneShot(lasersound);
        Destroy(gameObject,bullitLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

}
