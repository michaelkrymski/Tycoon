using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerItem : MonoBehaviour
{

    public static ConveyerItem Create(Vector3 worldPosition, ItemData item)
    {
        Transform conveyerItemTransform = Instantiate(item.itemPrefab, worldPosition, Quaternion.identity);

        ConveyerItem conveyerItem = conveyerItemTransform.GetComponent<ConveyerItem>();

        return conveyerItem;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void MoveSelf(Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }
}
