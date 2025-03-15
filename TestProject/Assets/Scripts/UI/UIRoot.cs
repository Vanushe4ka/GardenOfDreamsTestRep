using UnityEngine;

public class UIRoot : MonoBehaviour
{
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private Transform _sceneUIContainer;

    public void Awake()
    {
        HideLoadingPanel();
    }

    public void ShowLoadingPanel()
    {
        _loadingPanel.SetActive(true);
    }

    public void HideLoadingPanel()
    {
        _loadingPanel.SetActive(false);
    }

    public void SetSceneUI(Transform sceneUI)
    {
        ClearSceneUI();
        sceneUI.SetParent(_sceneUIContainer,false);
    }

    public void ClearSceneUI()
    {
        int childCount = _sceneUIContainer.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(_sceneUIContainer.GetChild(i).gameObject);
        }
    }
}
