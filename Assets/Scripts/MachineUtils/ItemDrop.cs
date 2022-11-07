using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private BuildingData.Dir dir;
    private int itemCount = 10;
    private Conveyer nextConveyer;
    [SerializeField] private ItemData item;
    private bool isDispensing;
    private int dispenseSpeed;

    private void Awake()
    {
        PlacedObject placedObject = GetComponent<PlacedObject>();
        dir = placedObject.GetDir();

        if(Conveyer.CheckForConveyer(Conveyer.possiblePositions[Conveyer.GetIndexFromDir(dir)], placedObject))
        {
            nextConveyer = Conveyer.GetConveyer(Conveyer.possiblePositions[Conveyer.GetIndexFromDir(dir)], placedObject);
        }
    }

    private void Update()
    {
        if(itemCount > 0)
        {
            ConveyerItem newItem = ConveyerItem.Create(transform.position, item);
            newItem.MoveSelf(transform.position, nextConveyer.transform.position, nextConveyer.GetSpeed());
        }
    }

    private IEnumerable DispenseItem(float duration)
    {
        float time = 0;
        while(time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        isDispensing = false;
    }
}
