using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] BuildingData building;
    private Grid<GridObject> grid;
    private BuildingData.Dir dir = default;

    void Start()
    {
        grid = new Grid<GridObject>(10, 10, 10f, Vector3.zero, (int x, int y, Grid<GridObject> g) => new GridObject(x, y, g));
    }

    public class GridObject
    {
        private int x;
        private int y;
        private Grid<GridObject> grid;
        private PlacedObject placedObject;

        public GridObject(int x, int y, Grid<GridObject> grid)
        {
            this.x = x;
            this.y = y;
            this.grid = grid;
        }

        public PlacedObject GetPlacedObject()
        {
            return placedObject;
        }

        public override string ToString()
        {
            return x + ", " + y + "\n" + placedObject;
        }

        public void SetObject(PlacedObject placedObject)
        {
            this.placedObject = placedObject;
            grid.TriggerUpdate(x, y);
        }
        
        public void ClearObject()
        {
            placedObject = null;
            grid.TriggerUpdate(x, y);
        }

        public bool CanBuild()
        {
            return placedObject == null;
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Build();
        }

        if(Input.GetMouseButtonDown(1))
        {
            Delete();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            dir = BuildingData.GetNextDir(dir);
        }
    }

    private void Build()
    {
        grid.GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);
        Vector3 pos = grid.GetWorldPosition(x, y);
        GridObject gridObject = grid.GetGridObject(x, y);
        List<Vector2Int> list = building.GetGridPositionList(new Vector2Int(x, y), dir, grid.GetWidth(), grid.GetHeight());

        if(list[0].x < 0 || list[0].y < 0)
        {
            UtilsClass.CreateWorldTextPopup("Can't build here!", UtilsClass.GetMouseWorldPosition());
            return;
        }

        foreach(Vector2Int coordinate in list)
        {
            if(!grid.GetGridObject(coordinate.x, coordinate.y).CanBuild())
            {
                UtilsClass.CreateWorldTextPopup("Can't build here!", UtilsClass.GetMouseWorldPosition());
                return;
            }
        }

        if(gridObject != null)
        {
            if(gridObject.CanBuild())
            {
                PlacedObject placedObject = PlacedObject.Create(pos + new Vector3(building.GetRotationOffset(dir).x, building.GetRotationOffset(dir).y, 0), new Vector2Int(x, y), dir, building, 10, 10);
                foreach(Vector2Int tile in list)
                {
                    grid.GetGridObject(tile.x, tile.y).SetObject(placedObject);
                }
            }
            else
            {
                UtilsClass.CreateWorldTextPopup("Can't build here!", UtilsClass.GetMouseWorldPosition());
            }
        }
    }

    private void Delete()
    {
        GridObject gridObject = grid.GetGridObject(UtilsClass.GetMouseWorldPosition());
        PlacedObject placedObject = gridObject.GetPlacedObject();
        if(placedObject != null)
        {
            placedObject.DestroySelf();
            List<Vector2Int> list = placedObject.GetGridPositionList();
            foreach(Vector2Int tile in list)
            {
                grid.GetGridObject(tile.x, tile.y).ClearObject();
            }
        }
    }
}

/*
    private Grid<bool> grid;

        void Start()
        {
            grid = new Grid<bool>(4, 4, 10, new Vector3(10, 10), () => false);

        }

        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
                grid.SetValue(mousePosition, !grid.GetValue(mousePosition));
            }
        }

*/