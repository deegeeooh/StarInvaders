using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPot : MonoBehaviour
{
    [SerializeField] GameObject coin_100;
    [SerializeField] GameObject coin_1000;


    private void Awake()
    {
        CheckSingleton();
    }

    private void CheckSingleton()
    {
        if (FindObjectsOfType<GoldPot>().Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }


    }

    public void Add100Coin()
    {
        GameObject GoldPotCoin = Instantiate(coin_100, new Vector3(Random.Range(6.5f, 6.9f), -0.75f, 0), Quaternion.identity);
        GoldPotCoin.transform.SetParent(gameObject.transform);
    }
    
    public void Add1000Coin()
    {
        GameObject GoldPotCoin = Instantiate(coin_1000, new Vector3(Random.Range(6.5f, 6.9f), -0.75f, 0), Quaternion.identity);
        GoldPotCoin.transform.SetParent(gameObject.transform);
    }

    public void ResetGoldPot()
    {
        Destroy(gameObject);
    }

        
}
