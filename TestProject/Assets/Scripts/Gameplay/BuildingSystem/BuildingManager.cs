using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private Building[] _buildingPrefabs;
    [SerializeField] private Vector2Int _matrixSize;
    [SerializeField] private VisualGrid _visualGrid;
    [SerializeField] private Grid _tilemapGrid;
    [SerializeField] private string _saveFilePath;

    public Building[] BuildingPrefabs => _buildingPrefabs;

    private Vector2Int _mouseCell;
    public Vector2Int MouseCell => _mouseCell;

    private ManagerState _currentState;

    public GridManager gridManager { get; private set; }

    public BuildingFactory buildingFactory { get; private set; }

    public static readonly Color redSelectColor = new Color(1, 0, 0, 0.75f);
    public static readonly Color greenSelectColor = new Color(0, 1, 0, 0.75f);
    
    public void Initialize()
    {
        gridManager = new GridManager(_matrixSize, _tilemapGrid.cellSize, _visualGrid);
        buildingFactory = new BuildingFactory(gridManager);
        List<Building> loadedBuildings = LoadBuildings(_saveFilePath);
        if (loadedBuildings != null && loadedBuildings.Count > 0)
        {
            gridManager.LoadBuildings(loadedBuildings);
        }
        SetNeutralState();
    }

    public void SetBuildState(Building building)
    {
        SetState(new BuildManagerState(this, building));
    }

    public void SetDestroyState()
    {
        SetState(new DestroyManagerState(this));
    }

    public void SetNeutralState()
    {
        SetState(new NeutralManagerState(this));
    }

    public void SetState(ManagerState newState)
    {
        if (_currentState != null)
        {
            _currentState.OnExit();
        }

        _currentState = newState;
        _currentState.OnEnter();
    }

    public void ShowGrid()
    {
        _visualGrid.gameObject.SetActive(true);
    }

    public void HideGrid()
    {
        _visualGrid.gameObject.SetActive(false);
    }

    public void OnMouseLeftClick()
    {
        _currentState.OnMouseClick();
    }

    public void OnMouseRightClick()
    {
        SetNeutralState();
    }

    public void OnMouseMove(Vector2 mousePos)
    {
        Vector2Int newMouseCell = gridManager.WorldToGridPoint(Camera.main.ScreenToWorldPoint(mousePos));
        if (_mouseCell != newMouseCell)
        {
            _mouseCell = newMouseCell;
            _currentState.OnChangeMouseCell(_mouseCell);

        }
    }

    public void SaveBuildings(string fileName)
    {
        List<BuildingSaveData> saveData = new List<BuildingSaveData>();

        foreach (var building in gridManager.Buildings)
        {
            int prefabIndex = building.PrefabID;
            if (prefabIndex == -1) continue;
            saveData.Add(new BuildingSaveData
            {
                ID = prefabIndex,
                PosInGridX = building.posInGrid.x,
                PosInGridY = building.posInGrid.y
            });
        }

        SaveDataWrapper saveWrapper = new SaveDataWrapper(saveData);
        string json = JsonUtility.ToJson(saveWrapper, true);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName, json);
    }

    public List<Building> LoadBuildings(string fileName)
    {
        List<Building> loadedBuildings = new List<Building>();
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataWrapper saveDataWrapper = JsonUtility.FromJson<SaveDataWrapper>(json);
            List<BuildingSaveData> saveData = saveDataWrapper.buildings;

            foreach (var data in saveData)
            {
                Building foundPrefab = null;
                foreach (var prefab in _buildingPrefabs)
                {
                    var buildingComponent = prefab.GetComponent<Building>();
                    if (buildingComponent != null && buildingComponent.PrefabID == data.ID)
                    {
                        foundPrefab = prefab;
                        break;
                    }
                }

                if (foundPrefab != null)
                {
                    Vector2Int pos = new Vector2Int(data.PosInGridX, data.PosInGridY);
                    Building newBuilding = buildingFactory.CreateBuilding(foundPrefab, pos);
                    loadedBuildings.Add(newBuilding);
                }
                else
                {
                    Debug.LogError($"Prefab with ID {data.ID} not found in buildingPrefabs list!");
                }
            }
        }

        return loadedBuildings;
    }

    private void OnApplicationQuit()
    {
        SaveBuildings(_saveFilePath);
    }

}
