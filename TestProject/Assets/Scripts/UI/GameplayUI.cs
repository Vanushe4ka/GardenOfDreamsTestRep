using UnityEngine;
class GameplayUI : MonoBehaviour
{
    [SerializeField] private BuildingButton buildingButtonPrefab;
    [SerializeField] private Transform buildingButtonContainer;

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
            BuildingButton button = Instantiate(buildingButtonPrefab.gameObject, buildingButtonContainer).GetComponent<BuildingButton>();
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
