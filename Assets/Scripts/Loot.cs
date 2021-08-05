using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] bool gold;
    [SerializeField] bool shooter_3way;
    [SerializeField] bool shooter_double;
    [SerializeField] bool life;
    [SerializeField] bool health;
    [SerializeField] bool bonusScore;
    [SerializeField] bool powerUp;
    [SerializeField] int value = 10;                     // value for either gold/health or life.
    [SerializeField] AudioClip lootSound;
    [Range(0f, 1f)] [SerializeField] float volume = 1f;

    AudioSource myAudiosource;

    // Start is called before the first frame update
    void Start()
    {
        myAudiosource = GetComponent<AudioSource>();
        myAudiosource.volume = volume;
        myAudiosource.PlayOneShot(lootSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IsGold() { return gold; }

    public bool IsShooter_3way() { return shooter_3way; }

    public bool IsShooter_Double() { return shooter_double; }

    public bool IsLife() { return life; }

    public bool IsHealth() { return health; }

    public bool IsBonusScore() { return bonusScore; }

    public bool IsPowerUp() { return powerUp; }

    public int GetItemValue() { return value; }

    public void DestroyLootItem() { Destroy(gameObject);}

    public void PlaySound() { AudioSource.PlayClipAtPoint(lootSound,Camera.main.transform.position,volume);}



}
