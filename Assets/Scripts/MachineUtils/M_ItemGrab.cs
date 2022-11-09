using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ItemGrab : MonoBehaviour
{
    private BuildingData.Dir dir;
    private Conveyer[] previousConveyers;
    private int possibleConveyers;
    [SerializeField] private ConveyerItem item;
    private Vector2Int position;
    private Vector3 worldPositionCentered;
    private bool isDestroying;
    private MachineInventory inventory;

    private void Start() 
    {
        PlacedObject placedObject = GetComponentInParent<PlacedObject>();
        inventory = GetComponentInParent<MachineInventory>();
        dir = placedObject.GetDir();
        position = placedObject.GetOrigin() + BuildingData.GetComponentOffset(dir);
        worldPositionCentered = placedObject.GetWorldPositionCentered(BuildingData.GetComponentOffset(dir));
        possibleConveyers = 2;
        previousConveyers = new Conveyer[possibleConveyers];

        int i = 0;
        foreach(Vector2Int possiblePosition in Conveyer.possiblePositions)
        {
            if(Conveyer.CheckForConveyer(position + possiblePosition, placedObject))
            {
                previousConveyers[i] = Conveyer.GetConveyer(position + possiblePosition, placedObject);
                i++;
            }
        }
    }

    private bool GetDirOffset(Conveyer possibleConveyer)
    {
        Vector2Int conveyerFront = BuildingData.GetComponentOffset(possibleConveyer.GetDir());
        PlacedObject output = Conveyer.CheckOneSpot(conveyerFront, GetComponentInParent<PlacedObject>());
        if(output != null)
        {
            if(output.GetComponentInChildren<M_ItemDrop>() != null)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public void LateStart()
    {
        Start();
    }

    private void Update()
    {
        foreach(Conveyer previousConveyer in previousConveyers)
        {
            if(previousConveyer != null)
            {
                if(previousConveyer.ConveyerIsOccupied())
                {
                    if(!isDestroying && !previousConveyer.GetItem().GetIsMoving())
                    {
                        item = previousConveyer.GetItem();
                        if(!inventory.GetIsFull(item.GetItem()))
                        {
                            item.MoveSelf(previousConveyer.GetWorldPositionCentered(), worldPositionCentered, previousConveyer.GetSpeed());
                            StartCoroutine(DestroyItem(previousConveyer));
                        }
                        
                    }
                }
            }
        }
    }

    private IEnumerator DestroyItem(Conveyer conveyer)
    {
        float time = 0;
        isDestroying = true;
        conveyer.RemoveItem();
        while(time < conveyer.GetSpeed())
        {
            time += Time.deltaTime;
            yield return null;
        }
        inventory.AddInventory(item.GetItem());
        item.DestroySelf();
        item = null;
        isDestroying = false;
    }
}
