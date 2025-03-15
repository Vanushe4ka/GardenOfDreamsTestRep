using System;
using UnityEngine.UI;
using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private Image _buildingImage;
    [SerializeField] private Button _button;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _selectedColor;

    private Building _buildingPrefab;

    public Building building => _buildingPrefab;

    public void Initialize(Building buildingPrefab, Action<BuildingButton> onBuildingSelected)
    {
        _buildingPrefab = buildingPrefab;
        _buildingImage.sprite = buildingPrefab.sprite;
        _button.image.color = _defaultColor;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => onBuildingSelected?.Invoke(this));
    }

    public void SetSelected(bool isSelected)
    {
        _button.image.color = isSelected ? _selectedColor : _defaultColor;
    }
    
}
