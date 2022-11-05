using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
    private PlacedObject placedObject;
    private BuildingData.Dir dir;
    private Vector2Int position;
    [SerializeField] private Conveyer nextConveyer; 
    private int conveyerIndex; // North, East, South, West.
    private List<Vector2Int> possiblePositions = new List<Vector2Int>(){new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0)};
    [SerializeField] private static int myInt;
    [SerializeField] private int myIntViewable;

    private bool isOccupied;

    public void Create()
    {
        myInt++;
        myIntViewable = myInt;
        placedObject = GetComponent<PlacedObject>();
        dir = placedObject.GetDir();
        position = placedObject.GetOrigin();

        int i = 0; // North, East, South, West.
        foreach(Vector2Int possiblePosition in possiblePositions)
        {
            if(CheckForConveyer(position + possiblePosition))
            {
                Conveyer conveyer = GetConveyer(position + possiblePosition);
                if(GetIndexFromDir(GetDir()) == i)
                {
                    conveyerIndex = i;
                    nextConveyer = conveyer;
                }
                if(GetIndexFromDir(GetDir()) == (i + 2) % 4)
                {
                    conveyer.SetNextConveyer(this);
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

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public Vector2Int GetPosition()
    {
        return position;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            List<string> nameList = new List<string>(){"North", "East", "South", "West"};
            Debug.Log(myInt + " " + nameList[conveyerIndex] + ": " + nextConveyer);
        }
    }
}