
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class BuildingFactory
{
    private GridManager _gridManager;
    public BuildingFactory(GridManager gridManager)
    {
        _gridManager = gridManager;
    }
    public Building CreateBuilding(Building prefab, Vector2Int posInGrid)
    {
        Vector3 buildingPos = _gridManager.GridToWorldPoint(posInGrid);
        Building building = Object.Instantiate(prefab.gameObject, buildingPos, Quaternion.identity).GetComponent<Building>();
        building.Initialize(posInGrid);
        return building;
    }
}
