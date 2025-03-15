using System.Collections.Generic;
using UnityEngine;
public class GridManager
{
    private int[,] _matrix;
    private Vector2Int _matrixSize;
    private Vector2 _cellSize;
    private VisualGrid _visualGrid;
    private List<Building> _buildings;

    public int[,] Matrix => _matrix;
    public List<Building> Buildings => _buildings;

    public GridManager(Vector2Int gridSize, Vector2 cellSize, VisualGrid visualGrid)
    {
        _matrixSize = gridSize;
        _cellSize = cellSize;
        _visualGrid = visualGrid;
        _buildings = new List<Building>();

        _matrix = new int[_matrixSize.y, _matrixSize.x];
        _visualGrid.DrawGrid(_matrix);
    }

    public void LoadBuildings(List<Building> buildings)
    {
        _buildings = buildings;
        CalcMatrixByBuildingList();
    }

    public void DrawMatrixWithBuilding(Building building)
    {
        if (!IsBuildingWithinBounds(building))  return; 

        int[,] tempMatrix = (int[,])_matrix.Clone();
        AddBuildingToMatrix(building, tempMatrix);
        _visualGrid.DrawGrid(tempMatrix);
    }

    public void RedrawGrid()
    {
        _visualGrid.DrawGrid(_matrix);
    }

    public void PlaceBuilding(Building building)
    {
        if (!IsCanPlaceBuilding(building))  return; 

        _buildings.Add(building);
        AddBuildingToMatrix(building, _matrix);
        _visualGrid.DrawGrid(_matrix);
    }

    public void AddBuildingToMatrix(Building building, int[,] matrix)
    {
        int[,] buildingMatrix = building.matrix;
        Vector2Int buildingPos = building.posInGrid;
        int buildingRows = buildingMatrix.GetLength(0);
        int buildingCols = buildingMatrix.GetLength(1);

        for (int i = 0; i < buildingRows; i++)
        {
            for (int j = 0; j < buildingCols; j++)
            {
                if (buildingMatrix[i, j] == 1)
                {
                    int mainMatrixRow = buildingPos.y + i;
                    int mainMatrixCol = buildingPos.x + j;

                    matrix[mainMatrixRow, mainMatrixCol] = 1;
                }
            }
        }
    }

    public Building GetBuildingAtPoint(Vector2Int gridPoint)
    {
        if (gridPoint.x < 0 || gridPoint.y < 0 ||
            gridPoint.x >= _matrix.GetLength(1) ||
            gridPoint.y >= _matrix.GetLength(0) ||
            _matrix[gridPoint.y, gridPoint.x] == 0)
        {
            return null;
        }

        for (int i = _buildings.Count - 1; i >= 0; i--)
        {
            Building building = _buildings[i];
            Vector2Int pos = building.posInGrid;
            int[,] matrix = building.matrix;
            int cols = matrix.GetLength(1);
            int rows = matrix.GetLength(0);

            if (gridPoint.x < pos.x || gridPoint.x >= pos.x + cols ||
                gridPoint.y < pos.y || gridPoint.y >= pos.y + rows)
            {
                continue;
            }

            int localX = gridPoint.x - pos.x;
            int localY = gridPoint.y - pos.y;

            if (matrix[localY, localX] == 1)
            {
                return building;
            }
        }

        return null;
    }

    public void DeleteBuilding(Building building)
    {
        _buildings.Remove(building);
        Object.Destroy(building.gameObject);

        CalcMatrixByBuildingList();
    }

    public void CalcMatrixByBuildingList()
    {
        _matrix = new int[_matrixSize.y, _matrixSize.x];

        foreach (Building building in _buildings)
        {
            AddBuildingToMatrix(building, _matrix);
        }

        _visualGrid.DrawGrid(_matrix);
    }

    public bool IsCanPlaceBuilding(Building building)
    {
        return IsBuildingWithinBounds(building)
            && !HasOverlappingUnits(building);
    }

    private bool IsBuildingWithinBounds(Building building)
    {
        int[,] buildingMatrix = building.matrix;
        Vector2Int buildingPos = building.posInGrid;

        int matrixRows = _matrix.GetLength(0);
        int matrixCols = _matrix.GetLength(1);
        int buildingRows = buildingMatrix.GetLength(0);
        int buildingCols = buildingMatrix.GetLength(1);

        return buildingPos.x >= 0
            && buildingPos.y >= 0
            && (buildingPos.x + buildingCols) <= matrixCols
            && (buildingPos.y + buildingRows) <= matrixRows;
    }

    private bool HasOverlappingUnits(Building building)
    {
        int[,] buildingMatrix = building.matrix;
        Vector2Int buildingPos = building.posInGrid;

        int buildingRows = buildingMatrix.GetLength(0);
        int buildingCols = buildingMatrix.GetLength(1);

        for (int i = 0; i < buildingRows; i++)
        {
            for (int j = 0; j < buildingCols; j++)
            {
                if (buildingMatrix[i, j] == 1)
                {
                    int mainY = buildingPos.y + i;
                    int mainX = buildingPos.x + j;

                    if (_matrix[mainY, mainX] == 1)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public Vector2 GridToWorldPoint(Vector2Int gridPoint)
    {
        return new Vector2(gridPoint.x, gridPoint.y) + _cellSize / 2;
    }

    public Vector2Int WorldToGridPoint(Vector3 worldPoint)
    {
        return Vector2Int.CeilToInt((worldPoint / _cellSize) - _cellSize);
    }
}
