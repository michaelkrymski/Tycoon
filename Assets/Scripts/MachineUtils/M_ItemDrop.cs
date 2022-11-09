using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ItemDrop : MonoBehaviour
{
    private BuildingData.Dir dir;
    private Conveyer nextConveyer;
    [SerializeField] private ItemData item;
    private bool isDispensing;
    private int dispenseSpeed;
    private Vector2Int position;
    private Vector3 worldPositionCentered;
    private MachineInventory inventory;

    private void Start()
    {
        PlacedObject placedObject = GetComponentInParent<PlacedObject>();
        inventory = GetComponentInParent<MachineInventory>();
        dir = placedObject.GetDir();
        position = placedObject.GetOrigin();
        worldPositionCentered = placedObject.GetWorldPositionCentered();

        if(Conveyer.CheckForConveyer(position + Conveyer.possiblePositions[Conveyer.GetIndexFromDir(dir)], placedObject))
        {
            nextConveyer = Conveyer.GetConveyer(position + Conveyer.possiblePositions[Conveyer.GetIndexFromDir(dir)], placedObject);
        }
    }

    public void LateStart()
    {
        Start();
    }

    private void Update()
    {
        if(!isDispensing && nextConveyer != null)
        {
            if(!nextConveyer.ConveyerIsOccupied())
            {
                if(inventory.GetAmountOfItem(item) > 0)
                {
                    StartCoroutine(DispenseItem(dispenseSpeed));
                    inventory.DecreaseItem(item);
                }
            }
        }
    }

    private IEnumerator DispenseItem(float duration)
    {
        float time = 0;
        isDispensing = true;
        ConveyerItem newItem = ConveyerItem.Create(transform.position, item);
        newItem.MoveSelf(worldPositionCentered, nextConveyer.GetWorldPositionCentered(), nextConveyer.GetSpeed());
        nextConveyer.SetItem(newItem);
        while(time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        isDispensing = false;
    }
}
