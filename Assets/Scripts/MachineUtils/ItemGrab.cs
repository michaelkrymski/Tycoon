using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrab : MonoBehaviour
{
    private BuildingData.Dir dir;
    [SerializeField] private int itemCount;
    private Conveyer previousConveyer;
    [SerializeField] private ConveyerItem item;
    private Vector2Int position;
    private Vector3 worldPositionCentered;
    private bool isDestroying;

    private void Start()
    {
        PlacedObject placedObject = GetComponent<PlacedObject>();
        dir = placedObject.GetDir();
        position = placedObject.GetOrigin();
        worldPositionCentered = placedObject.GetWorldPositionCentered();

        if(Conveyer.CheckForConveyer(position + Conveyer.possiblePositions[(Conveyer.GetIndexFromDir(dir) + 2) % 4], placedObject))
        {
            previousConveyer = Conveyer.GetConveyer(position + Conveyer.possiblePositions[Conveyer.GetIndexFromDir(dir) - 2], placedObject);
        }
    }

    private void Update()
    {
        if(previousConveyer != null)
        {
            if(previousConveyer.ConveyerIsOccupied())
            {
                if(!isDestroying)
                {
                    item = previousConveyer.GetItem();
                    item.MoveSelf(previousConveyer.GetWorldPositionCentered(), worldPositionCentered, previousConveyer.GetSpeed());
                    StartCoroutine(DestroyItem());
                }
            }
        }
    }

    private IEnumerator DestroyItem()
    {
        float time = 0;
        isDestroying = true;
        while(time < previousConveyer.GetSpeed())
        {
            time += Time.deltaTime;
            yield return null;
        }
        item.DestroySelf();
        item = null;
        previousConveyer.RemoveItem();
        itemCount++;
        isDestroying = false;
    }
}
