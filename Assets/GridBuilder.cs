using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] BuildingData building;
    private Grid<GridObject> grid;

    void Start()
    {
        grid = new Grid<GridObject>(10, 10, 10f, Vector3.zero, (int x, int y, Grid<GridObject> g) => new GridObject(x, y, g));
    }

    public class GridObject
    {
        private int x;
        private int y;
        private Grid<GridObject> grid;
        private Transform currentObject;

        public GridObject(int x, int y, Grid<GridObject> grid)
        {
            this.x = x;
            this.y = y;
            this.grid = grid;
        }

        public override string ToString()
        {
            return x + ", " + y + "\n" + currentObject;
        }

        public void SetObject(Transform obj)
        {
            currentObject = obj;
            grid.TriggerUpdate(x, y);
        }
        
        public void ClearObject()
        {
            currentObject = null;
            grid.TriggerUpdate(x, y);
        }

        public bool CanBuild()
        {
            return currentObject == null;
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            grid.GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);
            Vector3 pos = grid.GetWorldPosition(x, y);
            GridObject gridObject = grid.GetGridObject(x, y);
            List<Vector2Int> list = building.GetGridPositionList(new Vector2Int(x, y), default, grid.GetWidth(), grid.GetHeight());
            if(gridObject != null)
            {
                if(gridObject.CanBuild())
                {
                    Transform obj = Instantiate(building.buildingPrefab, pos, Quaternion.identity);
                    foreach(Vector2Int tile in list)
                    {
                        grid.GetGridObject(tile.x, tile.y).SetObject(obj);
                    }
                }
                else
                {
                    UtilsClass.CreateWorldTextPopup("Can't build here!", UtilsClass.GetMouseWorldPosition());
                }
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