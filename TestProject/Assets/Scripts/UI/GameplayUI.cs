using UnityEngine;
class GameplayUI : MonoBehaviour
{
    [SerializeField] private BuildingButton _buildingButtonPrefab;
    [SerializeField] private Transform _buildingButtonContainer;

    private BuildingManager _buildingManager;
    private BuildingButton _selectedButton;

    public void Initialize(BuildingManager buildingManager)
    {
        _buildingManager = buildingManager;
        InitializeBuildingButtons();
    }

    private void InitializeBuildingButtons()
    {
        foreach (var buildingPrefab in _buildingManager.BuildingPrefabs)
        {
            BuildingButton button = Instantiate(_buildingButtonPrefab.gameObject, _buildingButtonContainer).GetComponent<BuildingButton>();
            button.Initialize(buildingPrefab, OnBuildingSelected);
        }
    }

    private void OnBuildingSelected(BuildingButton selectedButton)
    {
        if (_selectedButton != null)
        {
            _selectedButton.SetSelected(false);
        }

        _selectedButton = selectedButton;
        _selectedButton.SetSelected(true);
    }

    public void OnBuildButtonClick()
    {
        if (_selectedButton == null || _selectedButton.building == null) { return; }
        _buildingManager.SetBuildState(_selectedButton.building);
    }

    public void OnDestroyButtonClick()
    {
        _buildingManager.SetDestroyState();
    }
}
