using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingData : ScriptableObject
{
    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            case Dir.North:
                return Dir.East;
            case Dir.East:
                return Dir.South;
            case Dir.South:
                return Dir.West;
            case Dir.West:
                return Dir.North;
            default:
                return Dir.North;
        }
    }

    public enum Dir
    {
        North,
        East,
        South,
        West
    }

    public string buildingName;
    public Transform buildingPrefab;
    public int width;
    public int height;

    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            case Dir.North:
                return 0;
            case Dir.West:
                return 90;
            case Dir.South:
                return 180;
            case Dir.East:
                return 270;
            default:
                return 0;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            case Dir.North:
                return new Vector2Int(0, 0);
            case Dir.East:
                return new Vector2Int(0, 1);
            case Dir.South:
                return new Vector2Int(1, 1);
            case Dir.West:
                return new Vector2Int(1, 0);
            default:
                return new Vector2Int(0, 0);
        }
    }

    

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir, int gridWidth, int gridHeight)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        Vector2Int rotationOffset;
        Vector2Int positionOffset = default;
        if(width > 1 && height > 1)
        {
            rotationOffset = GetRotationOffset(dir);
            positionOffset.x = width - 1;
            positionOffset.y = height - 1;
        }
        else
        {
            rotationOffset = default;
            positionOffset.x = 1;
            positionOffset.y = 1;
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridPositionList.Add(new Vector2Int(x, y) + offset - rotationOffset);
            }
        }
        foreach(Vector2Int position in gridPositionList)
        {
            if(position.x > gridWidth - positionOffset.x || position.y > gridHeight - positionOffset.y)
            {   
                return new List<Vector2Int>(){new Vector2Int(-1, -1)};
            }
        }
        return gridPositionList;
    }
}
