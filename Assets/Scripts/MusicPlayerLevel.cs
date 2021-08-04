using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerLevel : MonoBehaviour
{
    // Start is called before the first frame update
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
       
    }
}
