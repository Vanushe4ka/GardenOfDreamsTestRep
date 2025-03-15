using UnityEngine;
using UnityEngine.EventSystems;

class GameplayEntryPoint : MonoBehaviour
{
    [SerializeField] private GameplayUI _gameplayUIPrefab;
    [SerializeField] private BuildingManager _buildingManager;

    private GameplayUI _gameplayUI;
    private GameInput gameInput; 

    private bool _isOverUI;

    public void Initialize(UIRoot uiRoot)
    {
        gameInput = new GameInput();
        gameInput.Enable();

        _buildingManager.Initialize();

        gameInput.Gameplay.LeftMouseClick.performed += context =>
        {
            if (!_isOverUI)
            {
                _buildingManager.OnMouseLeftClick();
            }
        };
        gameInput.Gameplay.RightMouseClick.performed += context =>
        {
            if (!_isOverUI)
            {
                _buildingManager.OnMouseRightClick();
            }
        };
        gameInput.Gameplay.MousePosition.performed += context =>
        {
            Vector2 mousePos = context.ReadValue<Vector2>();
            _buildingManager.OnMouseMove(mousePos);
        };

        _gameplayUI = Instantiate(_gameplayUIPrefab).GetComponent<GameplayUI>();
        uiRoot.SetSceneUI(_gameplayUI.transform);
        _gameplayUI.Initialize(_buildingManager);
    }

    private void Update()
    {
        _isOverUI = IsPointerOverUI();
    }
    
    private bool IsPointerOverUI()
    {
        EventSystem eventSystem = EventSystem.current;
        return eventSystem != null &&
               eventSystem.IsPointerOverGameObject(PointerInputModule.kMouseLeftId);
    }
}
