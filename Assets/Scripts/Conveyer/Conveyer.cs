using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
    private PlacedObject placedObject;
    private BuildingData.Dir dir;
    private Vector2Int position;
    private Vector3 worldPositionCentered;
    [SerializeField] Conveyer nextConveyer; 
    private int conveyerIndex; // North, East, South, West.
    private List<Vector2Int> possiblePositions = new List<Vector2Int>(){new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0)};

    public ConveyerItem currentItem;
    private int conveyerSpeed = 1;
    [SerializeField] ItemData itemData;
    private static int myInt;
    public int index;

    public void Create()
    {
        myInt++;
        index = myInt;
        placedObject = GetComponent<PlacedObject>();
        dir = placedObject.GetDir();
        position = placedObject.GetOrigin();
        worldPositionCentered = placedObject.GetWorldPositionCentered();

        int i = 0; // North, East, South, West.
        foreach(Vector2Int possiblePosition in possiblePositions)
        {
            if(CheckForConveyer(position + possiblePosition))
            {
                Conveyer conveyer = GetConveyer(position + possiblePosition);
                if(GetIndexFromDir(conveyer.GetDir()) == i) // If the conveyer is placed in the same direction to this one.
                {
                    conveyerIndex = i;
                    nextConveyer = conveyer;
                }
                Debug.Log(conveyer.GetDir() + " " + GetIndexFromDir(conveyer.GetDir()) + " " + (i + 2) % 4);
                if(GetIndexFromDir(conveyer.GetDir()) == (i + 2) % 4) // If the conveyer is placed in the opposite direction to this one.
                {
                    conveyer.SetNextConveyer(this);
                    Debug.Log("South");
                }
                else if(GetIndexFromDir(conveyer.GetDir()) == (i + 1) % 4) // If the conveyer is placed in the right direction to this one.
                {
                    conveyer.SetNextConveyer(this);
                    Debug.Log("East");
                }
                else if(GetIndexFromDir(conveyer.GetDir()) == (i + 3) % 4) // If the conveyer is palced in the left direction to this one.
                {
                    conveyer.SetNextConveyer(this);
                    Debug.Log("West");
                }
            }
            i++;
        }
    }

    private bool CheckForConveyer(Vector2Int checkPos)
    {
        if(placedObject.GetBuilder().GetPlacedObject(checkPos) != null)
        {
            return true;
        }
        return false;
    }

    private Conveyer GetConveyer(Vector2Int checkPos)
    {
        return placedObject.GetBuilder().GetPlacedObject(checkPos).GetComponent<Conveyer>();
    }


    public void SetNextConveyer(Conveyer newConveyer)
    {
        nextConveyer = newConveyer;
    }

    public BuildingData.Dir GetDir()
    {
        return dir;
    }

    public int GetIndexFromDir(BuildingData.Dir dir)
    {
        switch (dir)
        {
            case BuildingData.Dir.North:
                return 0;
            case BuildingData.Dir.East:
                return 1;
            case BuildingData.Dir.South:
                return 2;
            case BuildingData.Dir.West:
                return 3;
            default:
                return 0;
        }
    }

    public bool ConveyerIsOccupied()
    {
        return currentItem != null;
    }

    public void SetItem(ConveyerItem item)
    {
        currentItem = item;
    }

    public void RemoveItem()
    {
        currentItem = null;
    }

    public Vector3 GetWorldPositionCentered()
    {
        return worldPositionCentered;
    }

    public Conveyer GetNextConveyer()
    {
        return nextConveyer;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            List<string> nameList = new List<string>(){"North", "East", "South", "West"};
            Debug.Log(nameList[conveyerIndex] + ": " + nextConveyer);
        }

        if(Input.GetKeyDown(KeyCode.C) && !ConveyerIsOccupied())
        {
            ConveyerItem item = ConveyerItem.Create(transform.position, itemData);
            currentItem = item;
            SetItem(item);
        }

        if(GetNextConveyer() != null)
        {
            if(ConveyerIsOccupied() && !nextConveyer.ConveyerIsOccupied() && !currentItem.GetIsMoving())
            {
                currentItem.SetCurrentConveyer(this);
                currentItem.MoveSelf(nextConveyer.GetWorldPositionCentered(), conveyerSpeed);
                nextConveyer.SetItem(currentItem);
                RemoveItem();
            }
        }

        // If building then RemoVeCurrentConveyer();
    }
}