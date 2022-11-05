using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
    private PlacedObject placedObject;
    private BuildingData.Dir dir;
    private Vector2Int position;
    private Conveyer nextConveyer; 
    private int conveyerIndex; // North, East, South, West.
    private List<Vector2Int> possiblePositions = new List<Vector2Int>(){new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0)};
    public int myInt;

    public void Create()
    {
        placedObject = GetComponent<PlacedObject>();
        dir = placedObject.GetDir();
        position = placedObject.GetOrigin();

        int i = 0;
        foreach(Vector2Int possiblePosition in possiblePositions)
        {
            if(CheckForConveyer(position + possiblePosition))
            {
                Debug.Log("Found conveyer");
                Conveyer conveyer = GetConveyer(position + possiblePosition);
                nextConveyer = conveyer;
                conveyerIndex = i;
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


    public void SetConveyerAtPos(Conveyer newConveyer, int pos)
    {
        nextConveyer = newConveyer;
    }

    public BuildingData.Dir GetDir()
    {
        return dir;
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