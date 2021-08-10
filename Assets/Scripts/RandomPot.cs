using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPot : MonoBehaviour
{
    [SerializeField] GameObject coin_PowerUP;
    [SerializeField] GameObject coin_special;
    private void Awake()
    {
        CheckSingleton();
    }

    private void CheckSingleton()
    {
        if (FindObjectsOfType<RandomPot>().Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSpecialCoin()
    {
        GameObject randomCoin = Instantiate(coin_special, new Vector3(Random.Range(-7.025f, -5.780f), -2.5f, 0), Quaternion.identity);
        randomCoin.transform.SetParent(gameObject.transform);
    }
    public void ResetRandomPot()
    {
        Destroy(gameObject);
    }

}
