using UnityEngine;
using UnityEngine.EventSystems;

class GameplayEntryPoint : MonoBehaviour
{
    [SerializeField] private GameplayUI _gameplayUIPrefab;
    [SerializeField] BuildingManager buildingManager;
    private GameplayUI _gameplayUI;
    private GameInput gameInput; 
    private bool _isOverUI;
    public void Initialize(UIRoot uiRoot)
    {
        gameInput = new GameInput();
        gameInput.Enable();

        buildingManager.Initialize();
        gameInput.Gameplay.LeftMouseClick.performed += context =>
        {
            if (!_isOverUI)
            {
                buildingManager.OnMouseLeftClick();
            }
        };
        gameInput.Gameplay.RightMouseClick.performed += context =>
        {
            if (!_isOverUI)
            {
                buildingManager.OnMouseRightClick();
            }
        };
        gameInput.Gameplay.MousePosition.performed += context =>
        {
            Vector2 mousePos = context.ReadValue<Vector2>();
            buildingManager.OnMouseMove(mousePos);
        };

        _gameplayUI = Instantiate(_gameplayUIPrefab).GetComponent<GameplayUI>();
        uiRoot.SetSceneUI(_gameplayUI.transform);
        _gameplayUI.Initialize(buildingManager);
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
