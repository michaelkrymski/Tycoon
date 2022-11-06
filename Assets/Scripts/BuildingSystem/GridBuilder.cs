using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] List<BuildingData> buildings;
    private BuildingData building;
    private Grid<GridObject> grid;
    private BuildingData.Dir dir = default;
    private bool buildMode;

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
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildMode();
        }
        if(buildMode)
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

            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetNextBuilding();
            }

            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetNextBuilding(1);
            }

            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetNextBuilding(2);
            }

            SetHoverEffect();
        }
    }

    private void ToggleBuildMode()
    {
        buildMode = !buildMode;
        if(buildMode)
        {
            if(building == null)
            {
                SetNextBuilding();
            }
            grid.CreateGridLines(5, true);
        }
        else
        {
            grid.DestroyGridLines();
            HoverEffect.Instance.DestroyHoverEffect();
        }
    }

    void LateUpdate()
    {
        HoverEffect.Instance.ReturnPosition();
    }

    private void SetNextBuilding(int buildingNumber = 0)
    {
        building = buildings[buildingNumber];
        HoverEffect.Instance.DestroyHoverEffect();
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
                PlacedObject placedObject = PlacedObject.Create(pos + new Vector3(building.GetRotationOffset(dir).x, building.GetRotationOffset(dir).y, 0) * grid.GetCellSize(), new Vector2Int(x, y), dir, building, 10, 10, this);
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

    private void SetHoverEffect()
    {
        grid.GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);
        bool allowHover = true;
        List<Vector2Int> list = building.GetGridPositionList(new Vector2Int(x, y), dir, grid.GetWidth(), grid.GetHeight());
        if(list[0].x < 0 || list[0].y < 0)
        {
            HoverEffect.Instance.DestroyHoverEffect();
            return;
        }
        foreach(Vector2Int coordinate in list)
        {
            if(!grid.GetGridObject(coordinate.x, coordinate.y).CanBuild())
            {
                allowHover = false;
            }
        }
        if(!allowHover)
        {
            HoverEffect.Instance.DestroyHoverEffect();
        }
        else
        {
            // HoverEffect.Instance.RefreshHoverEffect(building.buildingPrefab, pos, Quaternion.Euler(0, 0, building.GetRotationAngle(dir)));
            if(HoverEffect.Instance.ReturnPosition() == Vector3.zero)
            {
                Vector3 firstPos = grid.GetWorldPosition(x, y) + new Vector3(building.GetRotationOffset(dir).x, building.GetRotationOffset(dir).y, 0) * grid.GetCellSize();
                HoverEffect.Instance.RefreshHoverEffect(building.buildingPrefab, firstPos, Quaternion.Euler(0, 0, building.GetRotationAngle(dir)));
            }
            else
            {
                Vector3 currentPos = HoverEffect.Instance.ReturnPosition();
                Vector3 pos = grid.GetWorldPosition(x, y) + new Vector3(building.GetRotationOffset(dir).x, building.GetRotationOffset(dir).y, 0) * grid.GetCellSize();
                HoverEffect.Instance.SetPosition(Vector3.Lerp(currentPos, pos, 0.1f));
                Quaternion currentAngle = HoverEffect.Instance.ReturnAngle();
                HoverEffect.Instance.SetAngle(Quaternion.Lerp(currentAngle, Quaternion.Euler(0, 0, building.GetRotationAngle(dir)), 0.1f));
            }
        }
    }

    public PlacedObject GetPlacedObject(Vector2Int coordinate)
    {
        if(grid.GetGridObject(coordinate.x, coordinate.y) != null)
        {
            return grid.GetGridObject(coordinate.x, coordinate.y).GetPlacedObject();
        }
        return null;
    }

    public Grid<GridObject> GetGrid()
    {
        return grid;
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