using System;
using UnityEngine.UI;
using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] Image buildingImage;
    [SerializeField] Button button;
    [SerializeField] Color defaultColor;
    [SerializeField] Color selectedColor;
    private Building _buildingPrefab;

    public Building building => _buildingPrefab;

    public void Initialize(Building buildingPrefab, Action<BuildingButton> onBuildingSelected)
    {
        _buildingPrefab = buildingPrefab;
        buildingImage.sprite = buildingPrefab.sprite;
        button.image.color = defaultColor;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onBuildingSelected?.Invoke(this));
    }

    public void SetSelected(bool isSelected)
    {
        button.image.color = isSelected ? selectedColor : defaultColor;
    }
    
}
