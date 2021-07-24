using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot Table")]

public class LootTable : ScriptableObject
{
    [SerializeField] List<ScriptableObject> lootItem;
    [Range(0, 100)] [SerializeField] int dropChance;


public List<ScriptableObject> GetLootItems()
    {

        return lootItem;
    }

public int GetLootTableDropChance () { return dropChance;  }




}




