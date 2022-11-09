using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MachineInventory : MonoBehaviour
{
    private Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    [SerializeField] private List<int> itemCounts;

    private void Awake()
    {
        SetupInventory(2, new ItemData[]{ItemManager.GetItemAtIndex("BlackSquareTest"), ItemManager.GetItemAtIndex("RedSquareTest")});
    }

    private void SetupInventory(int ingredientCount, ItemData[] ingredients)
    {
        itemCounts = new List<int>{};
        for (int i = 0; i < ingredientCount; i++)
        {
            inventory.Add(ingredients[i], 0);
            itemCounts.Add(0);
        }
    }

    public void AddInventory(ItemData item, int amount = 1)
    {
        inventory[item] += amount;
        UpdateDebug(item);
    }

    public int GetAmountOfItem(ItemData item)
    {
        return inventory[item];
    }

    public void DecreaseItem(ItemData item, int amount = 1)
    {
        inventory[item] -= amount;      
        UpdateDebug(item);
    }

    public void UpdateDebug(ItemData item)
    {
        foreach(ItemData key in inventory.Keys)
        {
            if (key == item)
            {
                itemCounts[inventory.Keys.ToList().IndexOf(item)] = inventory[key];
            }
        }
    }
}
