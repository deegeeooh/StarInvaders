using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerLevel : MonoBehaviour
{
    // Start is called before the first frame update

    bool sound = true;

    void Start()
    {
        CheckSingleton();
    }

    private void CheckSingleton()
    {
        if (FindObjectsOfType<MusicPlayerLevel>().Length > 1 )
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
        //if (Input.GetKeyDown(KeyCode.M ))            // reset highscore
        //{
        //    if (sound)
        //    {
        //        sound = false;
        //    }
        //    else if (!sound)
        //    {
        //        sound = true;
        //    }

        //    gameObject.GetComponent<AudioSource>().mute = sound;
        //    //Camera.main.GetComponent<AudioListener>().enabled = sound;
        //    //gameObject.SetActive(sound);
        //}
    }
}
