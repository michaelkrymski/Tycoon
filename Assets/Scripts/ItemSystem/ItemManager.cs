using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static Dictionary<string, ItemData> items = new Dictionary<string, ItemData>();
    [SerializeField] private ItemData[] itemData;

    private void Start()
    {
        ItemData[] itemDatas = Resources.LoadAll<ItemData>("Items");
        itemData = itemDatas;
        for(int i = 0; i < itemDatas.Length; i++)
        {
            items.Add(itemDatas[i].itemName, itemDatas[i]);
        }
    }

    public static ItemData GetItemAtIndex(string index)
    {
        return items[index];
    }
}