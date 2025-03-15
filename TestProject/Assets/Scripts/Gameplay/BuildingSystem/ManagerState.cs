using UnityEngine;

public abstract class ManagerState
{
    protected BuildingManager _buildingManager;
    protected Building _selectedBuilding;
    protected GridManager _gridManager;

    public ManagerState(BuildingManager buildingManager)
    {
        _buildingManager = buildingManager;
        _gridManager = buildingManager.gridManager;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnChangeMouseCell(Vector2Int mouseCell);
    public abstract void OnMouseClick();
}
