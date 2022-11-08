using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private List<ItemData> items;

    public ItemData GetItemAtIndex(int index)
    {
        return items[index];
    }

}