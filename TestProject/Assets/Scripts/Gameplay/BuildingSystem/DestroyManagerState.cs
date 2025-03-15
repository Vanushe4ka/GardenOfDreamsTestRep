using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class DestroyManagerState : ManagerState
{
    public DestroyManagerState(BuildingManager buildingManager) : base(buildingManager) { }

    public override void OnEnter()
    {
        _buildingManager.ShowGrid();
    }

    public override void OnExit()
    {
        _buildingManager.HideGrid();

        if (_selectedBuilding != null)
        {
            _selectedBuilding.Unselect();
        }
    }

    public override void OnMouseClick()
    {
        if (_selectedBuilding != null)
        {
            _gridManager.DeleteBuilding(_selectedBuilding);
            _selectedBuilding = null;
            _buildingManager.SetNeutralState();
        }
    }

    public override void OnChangeMouseCell(Vector2Int mouseCell)
    {
        Building buildingUnderMouse = _gridManager.GetBuildingAtPoint(mouseCell);

        if (buildingUnderMouse != _selectedBuilding && _selectedBuilding != null)
        {
            _selectedBuilding.Unselect();
        }
        
        if (buildingUnderMouse != null)
        {
            buildingUnderMouse.Select(BuildingManager.redSelectColor);
        }

        _selectedBuilding = buildingUnderMouse;
    }
}
