using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid<TGridObject>
{
    private int width, height;
    private float cellSize;
    private Vector3 origin;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;
    private GameObject gridBorderLines;
    private GameObject gridLines;
    private bool debug;

    // Constructor
    public Grid(int width, int height, float cellSize, Vector3 origin, Func<int, int, Grid<TGridObject>, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        gridArray = new TGridObject[width, height];
        debug = false;

        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(x, y, this);
            }
        }

        if(debug)
        {
            debugTextArray = new TextMesh[width, height];
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize / 2, cellSize / 2), 20, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        }

    }

    public void CreateGridLines(int border, bool interior)
    {
        if(gridBorderLines == null)
        {
            gridBorderLines = new GameObject("GridBorder");
            gridBorderLines.transform.position = origin;
            LineRenderer gridBorder = gridBorderLines.AddComponent<LineRenderer>();
            gridBorder.material = new Material(Shader.Find("Sprites/Default"));
            gridBorder.startColor = Color.white;
            gridBorder.startWidth = 0.3f;
            gridBorder.positionCount = 5;
            // External lines
            gridBorder.SetPosition(0, GetWorldPosition(0, 0) + new Vector3(-border, -border));
            gridBorder.SetPosition(1, GetWorldPosition(width, 0) + new Vector3(border, -border));
            gridBorder.SetPosition(2, GetWorldPosition(width, height) + new Vector3(border, border));
            gridBorder.SetPosition(3, GetWorldPosition(0, height) + new Vector3(-border, border));
            gridBorder.SetPosition(4, GetWorldPosition(0, 0) + new Vector3(-border, -border));
        }
        // Internal lines
        if(interior && gridLines == null)
        {
            gridLines = new GameObject("GridLines");
            gridLines.transform.position = origin;
            LineRenderer lines = gridLines.AddComponent<LineRenderer>();
            lines.material = new Material(Shader.Find("Sprites/Default"));
            lines.startColor = Color.white;
            lines.startWidth = 0.3f;
            lines.positionCount = GetWidth() * GetHeight() * 5 + GetWidth() * 2;
            int iterations = 0;

            for (int x = 0; x < GetWidth(); x++)
            {
                for (int y = 0; y < GetHeight(); y++)
                {
                    lines.SetPosition(iterations, GetWorldPosition(x, y));
                    iterations++;
                    lines.SetPosition(iterations, GetWorldPosition(x, y + 1));
                    iterations++;
                    lines.SetPosition(iterations, GetWorldPosition(x + 1, y + 1));
                    iterations++;
                    lines.SetPosition(iterations, GetWorldPosition(x + 1, y));
                    iterations++;
                    lines.SetPosition(iterations, GetWorldPosition(x, y));
                    iterations++;
                } // Five iterations per square. 

                lines.SetPosition(iterations, GetWorldPosition(x, GetHeight()));
                iterations++;
                lines.SetPosition(iterations, GetWorldPosition(x + 1, GetHeight()));
                iterations++;
            } // Two iterations per column. 
        }
    }

    public void DestroyGridLines()
    {
        GameObject.Destroy(gridLines);
    }

    public void TriggerUpdate(int x, int y)
    {
        if(debug)
        {
            debugTextArray[x, y].text = gridArray[x, y]?.ToString();
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + origin;
    }

    // Set the value of a grid cell using x, y format. 
    public void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y]?.ToString();
        }
    }

    // Set the value of a grid cell using a Vector3 position.
    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    // Get the value of a grid cell.
    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    // Get the value of a grid cell using world coordinates.
    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    // Get the position in the grid from a world position.
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - origin).y / cellSize);
    }

    // Get width of grid.
    public int GetWidth()
    {
        return width;
    }

    // Get height of grid.
    public int GetHeight()
    {
        return height;
    }

    // Get the size of a grid cell.
    public float GetCellSize()
    {
        return cellSize;
    }

    // Get the origin of the grid.
    public Vector3 GetOrigin()
    {
        return origin;
    }
}
