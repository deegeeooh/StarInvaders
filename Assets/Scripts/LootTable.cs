using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot Table")]

public class LootTable : ScriptableObject
{
    [SerializeField] List<LootItem> lootItem;               // LootItem is a scriptableObject
    [SerializeField] bool multipleDropsPerRoll = false;
    [Range(0, 100)] [SerializeField] int dropChance;


public List<LootItem> GetLootItems()
    {
        var items = new List<LootItem>();

        foreach (LootItem child in lootItem)
        {
            items.Add(child);
        }
        return items;
    }

public int GetLootTableDropChance () { return dropChance;  }

public bool CanMultipleDropPerRoll () { return multipleDropsPerRoll; }





}




