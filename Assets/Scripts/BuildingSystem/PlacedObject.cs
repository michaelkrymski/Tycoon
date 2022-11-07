using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    private BuildingData building;
    private Vector2Int origin;
    private BuildingData.Dir dir;
    private int gridWidth;
    private int gridHeight;
    private GridBuilder gridBuilder;

    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, BuildingData.Dir dir, BuildingData building, int gridWidth, int gridHeight, GridBuilder gridBuilder)
    {
        Transform placedObjectTransform = Instantiate(building.buildingPrefab, worldPosition, Quaternion.Euler(0, 0, building.GetRotationAngle(dir)));

        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
        placedObject.building = building;
        placedObject.origin = origin;
        placedObject.dir = dir;
        placedObject.gridWidth = gridWidth;
        placedObject.gridHeight = gridHeight;
        placedObject.gridBuilder = gridBuilder;

        return placedObject;
    }

    public List<Vector2Int> GetGridPositionList()
    {
        List<Vector2Int> gridPositionList = building.GetGridPositionList(origin, dir, gridWidth, gridHeight);        
        return gridPositionList;
    }

    public BuildingData.Dir GetDir()
    {
        return dir;
    }

    public GridBuilder GetBuilder()
    {
        return gridBuilder;
    }

    public Vector2Int GetOrigin()
    {
        return origin;
    }

    public Vector3 GetWorldPositionCentered()
    {
        return gridBuilder.GetGrid().GetWorldPositionCentralized(origin.x, origin.y);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
