using UnityEngine;
class BuildManagerState : ManagerState
{
    public BuildManagerState(BuildingManager buildingManager, Building selectedBuilding) : base(buildingManager)
    {
        Vector2Int cell = buildingManager.MouseCell;
        _selectedBuilding = _buildingManager.buildingFactory.CreateBuilding(selectedBuilding, cell);
    }

    public override void OnEnter()
    {
        _buildingManager.ShowGrid();
    }

    public override void OnExit()
    {
        if (_selectedBuilding != null && _selectedBuilding.gameObject != null)
        {
            Object.Destroy(_selectedBuilding.gameObject);
        }

        _gridManager.RedrawGrid();
        _buildingManager.HideGrid();
    }

    public override void OnMouseClick()
    {
        if (_gridManager.IsCanPlaceBuilding(_selectedBuilding))
        {
            _gridManager.PlaceBuilding(_selectedBuilding);
            _selectedBuilding.Unselect();
            _selectedBuilding = null;
            _buildingManager.SetNeutralState();
        }
    }

    public override void OnChangeMouseCell(Vector2Int mouseCell)
    {
        Vector3 buildingPos = _gridManager.GridToWorldPoint(mouseCell);
        _selectedBuilding.transform.position = buildingPos;
        _selectedBuilding.posInGrid = mouseCell;
        _selectedBuilding.Select((_gridManager.IsCanPlaceBuilding(_selectedBuilding) ? BuildingManager.greenSelectColor : BuildingManager.redSelectColor));
        _selectedBuilding.DefineOrderLayer();
        _gridManager.DrawMatrixWithBuilding(_selectedBuilding);
    }
}
