using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot Item")]


public class LootItem : ScriptableObject
{
    [SerializeField] GameObject lootItem;
    [Range(0, 100)] [SerializeField] int weight;
  
    [SerializeField] float itemDropSpeed = 2f;

    public GameObject GetLootItem() { return lootItem; }

    public int GetItemWeight() { return weight; }
    
    public float GetItemdropSpeed() { return itemDropSpeed;}

}
