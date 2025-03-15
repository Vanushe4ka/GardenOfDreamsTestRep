using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Bootstrap
{
    private static Bootstrap _instance;
    private BootstrapCoroutines _coroutines;
    private UIRoot _uiRoot;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AutostartGame()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        _instance = new Bootstrap();
        _instance.StartGame();
    }

    private Bootstrap()
    {
        _coroutines = new GameObject("COROUTINES").AddComponent<BootstrapCoroutines>();
        Object.DontDestroyOnLoad(_coroutines.gameObject);

        UIRoot prefabUIRoot = Resources.Load<UIRoot>("UIRoot");
        _uiRoot = Object.Instantiate(prefabUIRoot);
        Object.DontDestroyOnLoad(_uiRoot.gameObject);
    }

    private void StartGame()
    {

#if UNITY_EDITOR
        string SceneName = SceneManager.GetActiveScene().name;
        if (SceneName == "Gameplay")
        {
            _coroutines.StartCoroutine(LoadGameplay());
            return;
        }
        if (SceneName != "Boot")
        {
            return;
        }

#endif

        _coroutines.StartCoroutine(LoadGameplay());
    }

    private IEnumerator LoadGameplay()
    {
        _uiRoot.ShowLoadingPanel();

        yield return SceneManager.LoadSceneAsync("Boot");
        yield return SceneManager.LoadSceneAsync("Gameplay");
        GameplayEntryPoint gameplayEntryPoint = Object.FindObjectOfType<GameplayEntryPoint>();
        gameplayEntryPoint.Initialize(_uiRoot);

        yield return null;

        _uiRoot.HideLoadingPanel();
    }
}
