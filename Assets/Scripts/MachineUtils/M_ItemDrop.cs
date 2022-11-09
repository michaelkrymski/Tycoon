using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ItemDrop : MonoBehaviour
{
    private BuildingData.Dir dir;
    private Conveyer[] nextConveyers;
    [SerializeField] private ItemData item;
    private bool isDispensing;
    private int dispenseSpeed;
    private Vector2Int position;
    private Vector3 worldPositionCentered;
    private MachineInventory inventory;
    private int productIndex;

    private void Start()
    {
        PlacedObject placedObject = GetComponentInParent<PlacedObject>();
        inventory = GetComponentInParent<MachineInventory>();
        dir = placedObject.GetDir();
        position = placedObject.GetOrigin();
        worldPositionCentered = placedObject.GetWorldPositionCentered();
        nextConveyers = new Conveyer[2];

        int i = 0;
        foreach(Vector2Int possiblePosition in Conveyer.possiblePositions)
        {
            if(Conveyer.CheckForConveyer(position + possiblePosition, placedObject))
            {
                nextConveyers[i] = Conveyer.GetConveyer(position + possiblePosition, placedObject);
                i++;
            }
        }
    }

    public void LateStart()
    {
        Start();
    }

    private void Update()
    {
        foreach(Conveyer nextConveyer in nextConveyers)
        {
            if(!isDispensing && nextConveyer != null)
            {
                if(!nextConveyer.ConveyerIsOccupied())
                {
                    if(item != null)
                    {
                        if(inventory.GetAmountOfItem(item) > 0)
                        {
                            StartCoroutine(DispenseItem(dispenseSpeed, nextConveyer));
                            inventory.DecreaseItem(item);
                        }
                        else
                        {
                            item = null;
                        }
                    }
                    else
                    {
                        item = inventory.GetNextProduct(productIndex);
                        productIndex %= 1 + inventory.GetProductCount();
                    }
                }
            }
        }
    }

    private IEnumerator DispenseItem(float duration, Conveyer conveyer)
    {
        float time = 0;
        isDispensing = true;
        ConveyerItem newItem = ConveyerItem.Create(transform.position, item);
        newItem.MoveSelf(worldPositionCentered, conveyer.GetWorldPositionCentered(), conveyer.GetSpeed());
        conveyer.SetItem(newItem);
        while(time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        isDispensing = false;
    }
}
