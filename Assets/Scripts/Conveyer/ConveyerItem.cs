using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerItem : MonoBehaviour
{
    private bool isMoving;
    private Conveyer currentConveyer;
    
    public static ConveyerItem Create(Vector3 worldPosition, ItemData item)
    {
        Transform conveyerItemTransform = Instantiate(item.itemPrefab, worldPosition, Quaternion.identity);

        ConveyerItem conveyerItem = conveyerItemTransform.GetComponent<ConveyerItem>();

        return conveyerItem;
    }

    public void DestroySelf(float delay = 0)
    {
        Destroy(gameObject, delay);
    }

    public void MoveSelf(Vector2 initialPosition, Vector2 worldPosition, int speed)
    {
        isMoving = true;
        StartCoroutine(MoveItem(initialPosition, worldPosition, speed));
    }

    public void SetCurrentConveyer(Conveyer conveyer)
    {
        currentConveyer = conveyer;
    }

    public void RemoveCurrentConveyer()
    {
        currentConveyer = null;
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    private IEnumerator MoveItem(Vector2 initialPosition, Vector2 targetPosition, float duration)
    {
        float time = 0;
        transform.position = initialPosition;
        Vector3 startPosition = new Vector3(transform.position.x, transform.position.y, -1);
        Vector3 newTargetPosition = new Vector3(targetPosition.x, targetPosition.y, -1);
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, newTargetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = newTargetPosition;
        isMoving = false;
        if(currentConveyer != null)
        {
            SetCurrentConveyer(currentConveyer.GetNextConveyer());
        }
    }
}
