using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{

    [SerializeField] float backgroundScrollSpeed = 0.01f;
    Material myMaterial;
    Vector2 offSet;
    float loopGameSpeed;


    // Start is called before the first frame update
    void Start()
    {

        CheckSingleton();                                                   // introduce Singleton so background scroll
        myMaterial = GetComponent<Renderer>().material;                     // doesn't reset with scene change
        offSet = new Vector2(0f, backgroundScrollSpeed);                    // x = 0 for top down scrolling
    }

    private void CheckSingleton()
    {
        if (FindObjectsOfType<BackgroundScroller>().Length > 1)
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
        myMaterial.mainTextureOffset += offSet * Time.deltaTime;
    }

    public void SetGameSpeed(float gamespeed)
    {
        offSet = new Vector2(0f, gamespeed);
    }

    public void Reset()
    {
        Destroy(gameObject);
    }


}
