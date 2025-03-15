using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private Building[] buildingPrefabs;
    public Building[] BuildingPrefabs => buildingPrefabs;

    private ManagerState _currentState;

    [SerializeField] Vector2Int matrixSize;
    [SerializeField] private VisualGrid visualGrid;

    [SerializeField] private Grid tilemapGrid;
    public GridManager gridManager { get; private set; }
    public BuildingFactory buildingFactory { get; private set; }

    private Vector2Int _mouseCell;
    public Vector2Int MouseCell => _mouseCell;

    public static readonly Color redSelectColor = new Color(1, 0, 0, 0.75f);
    public static readonly Color greenSelectColor = new Color(0, 1, 0, 0.75f);
    [SerializeField] string saveFilePath;

    public void Initialize()
    {
        gridManager = new GridManager(matrixSize, tilemapGrid.cellSize, visualGrid);
        buildingFactory = new BuildingFactory(gridManager);
        List<Building> loadedBuildings = LoadBuildings(saveFilePath);
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
        visualGrid.gameObject.SetActive(true);
    }
    public void HideGrid()
    {
        visualGrid.gameObject.SetActive(false);
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
                // Поиск префаба по ID
                Building foundPrefab = null;
                foreach (var prefab in buildingPrefabs)
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
        SaveBuildings(saveFilePath);
    }

}
