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
    public static List<Vector2Int> possiblePositions = new List<Vector2Int>(){new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0)};

    public ConveyerItem currentItem;
    private int conveyerSpeed = 1;

    private void Start()
    {
        placedObject = GetComponent<PlacedObject>();
        dir = placedObject.GetDir();
        position = placedObject.GetOrigin();
        worldPositionCentered = placedObject.GetWorldPositionCentered();

        int i = 0; // North, East, South, West.
        foreach(Vector2Int possiblePosition in possiblePositions)
        {
            if(CheckForConveyer(position + possiblePosition, placedObject))
            {
                Conveyer conveyer = GetConveyer(position + possiblePosition, placedObject);

                if(GetIndexFromDir(GetDir()) == i)
                {
                    SetNextConveyer(conveyer);
                }

                switch(i)
                {
                    case 0:
                        if(conveyer.GetDir() == BuildingData.Dir.South)
                        {
                            conveyer.SetNextConveyer(this);
                        }
                        break;
                    case 1:
                        if(conveyer.GetDir() == BuildingData.Dir.West)
                        {
                            conveyer.SetNextConveyer(this);
                        }
                        break;
                    case 2:
                        if(conveyer.GetDir() == BuildingData.Dir.North)
                        {
                            conveyer.SetNextConveyer(this);
                        }
                        break;
                    case 3:
                        if(conveyer.GetDir() == BuildingData.Dir.East)
                        {
                            conveyer.SetNextConveyer(this);
                        }
                        break;
                }
            }
            i++;

            CheckForExtras(position + possiblePosition, placedObject, out M_ItemDrop mDropper, out M_ItemGrab mGrabber, out ItemDrop dropper, out ItemGrab grabber);
            if(dropper != null)
            {
                dropper.LateStart();
            }
            if(grabber != null)
            {
                grabber.LateStart();
            }
            if(mDropper != null)
            {
                mDropper.LateStart();
            }
            if(mGrabber != null)
            {
                mGrabber.LateStart();
            }
        }
    }

    public static bool CheckForConveyer(Vector2Int checkPos, PlacedObject placedObject)
    {
        if(placedObject.GetBuilder().GetPlacedObject(checkPos) != null)
        {
            if(placedObject.GetBuilder().GetPlacedObject(checkPos).GetComponent<Conveyer>() != null)
            {
                return true;
            }
        }
        return false;
    }

    public static void CheckForExtras(Vector2Int checkPos, PlacedObject placedObject, out M_ItemDrop mDropper, out M_ItemGrab mGrabber, out ItemDrop dropper, out ItemGrab grabber)
    {
        if(placedObject.GetBuilder().GetPlacedObject(checkPos) != null)
        {
            if(placedObject.GetBuilder().GetPlacedObject(checkPos).GetComponent<MachineInventory>() != null)
            {
                dropper = null;
                grabber = null;
                if(placedObject.GetBuilder().GetPlacedObject(checkPos).GetComponentInChildren<M_ItemDrop>() != null)
                {
                    mDropper = placedObject.GetBuilder().GetPlacedObject(checkPos).GetComponentInChildren<M_ItemDrop>();
                }
                else
                {
                    mDropper = null;
                }
                if(placedObject.GetBuilder().GetPlacedObject(checkPos).GetComponentInChildren<M_ItemGrab>() != null)
                {
                    mGrabber = placedObject.GetBuilder().GetPlacedObject(checkPos).GetComponentInChildren<M_ItemGrab>();
                }
                else
                {
                    mGrabber = null;
                }
            }
            else
            {
                mDropper = null;
                mGrabber = null;
                if(placedObject.GetBuilder().GetPlacedObject(checkPos).GetComponent<ItemDrop>() != null)
                {
                    dropper = placedObject.GetBuilder().GetPlacedObject(checkPos).GetComponent<ItemDrop>();
                }
                else
                {
                    dropper = null;
                }
                if(placedObject.GetBuilder().GetPlacedObject(checkPos).GetComponent<ItemGrab>() != null)
                {
                    grabber = placedObject.GetBuilder().GetPlacedObject(checkPos).GetComponent<ItemGrab>();
                }
                else
                {
                    grabber = null;
                }
            }
        }
        else
        {
            dropper = null;
            grabber = null;
            mDropper = null;
            mGrabber = null;
        }
    }

    public static PlacedObject CheckOneSpot(Vector2Int checkPos, PlacedObject placedObject)
    {
        if(placedObject.GetBuilder().GetPlacedObject(checkPos) != null)
        {
            return placedObject.GetBuilder().GetPlacedObject(checkPos);
        }
        return null;
    }

    public static Conveyer GetConveyer(Vector2Int checkPos, PlacedObject placedObject)
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

    public static int GetIndexFromDir(BuildingData.Dir dir)
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

    public ConveyerItem GetItem()
    {
        return currentItem;
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

    public int GetSpeed()
    {
        return conveyerSpeed;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            List<string> nameList = new List<string>(){"North", "East", "South", "West"};
        }

        if(GetNextConveyer() != null)
        {
            if(ConveyerIsOccupied() && !nextConveyer.ConveyerIsOccupied() && !currentItem.GetIsMoving())
            {
                currentItem.SetCurrentConveyer(this);
                currentItem.MoveSelf(GetWorldPositionCentered(), nextConveyer.GetWorldPositionCentered(), conveyerSpeed);
                nextConveyer.SetItem(currentItem);
                RemoveItem();
            }
        }

        // If building then RemoVeCurrentConveyer();
    }
}