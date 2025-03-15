using UnityEngine;

public class NeutralManagerState : ManagerState
{
    public NeutralManagerState(BuildingManager buildingManager) : base(buildingManager) { }

    public override void OnEnter()
    {
        _buildingManager.HideGrid();
    }

    public override void OnExit(){}

    public override void OnMouseClick(){}

    public override void OnChangeMouseCell(Vector2Int mouseCell) { }

}
