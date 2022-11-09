using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MachineInventory : MonoBehaviour
{
    private Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    [SerializeField] private List<int> itemCounts;
    private List<ItemData> products;

    private void Awake()
    {
        //SetupInventory(2, new ItemData[]{ItemManager.GetItemAtIndex("BlackSquareTest"), ItemManager.GetItemAtIndex("RedSquareTest")}, new ItemData[]{});
    }

    public void SetupInventory(ItemData[] ingredients, ItemData[] products)
    {
        itemCounts = new List<int>{};
        this.products = products.ToList();
        ItemData[] allItems = ingredients.Concat(products).ToArray();
        int itemCount = allItems.Length;
        for (int i = 0; i < itemCount; i++)
        {
            inventory.Add(allItems[i], 0);
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

    public bool GetIsFull(ItemData item, int amount = 1)
    {
        return inventory[item] + amount > item.maxStackSize;
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

    public ItemData GetNextProduct(int index)
    {
        return products[index];
    }

    public int GetProductCount()
    {
        return products.Count;
    }
}
