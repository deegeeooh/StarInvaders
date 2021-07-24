using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot Item")]


public class LootItem : ScriptableObject
{
    [SerializeField] GameObject lootItem;
    [Range(0, 100)] [SerializeField] int dropChance;
    [SerializeField] bool gold;
    [SerializeField] bool shooter_3way;
    [SerializeField] bool shooter_double;
    [SerializeField] bool life;
    [SerializeField] bool health;

    public GameObject GetLootItem() { return lootItem; }

    public int GetDropChance() { return dropChance; }
    public bool IsGold () { return gold; }

    public bool IsShooter_3way () { return shooter_3way; }

    public bool IsShooter_Double() { return shooter_double; }

    public bool IsLife() { return life; }

    public bool IsHealth() { return health;  }


}
