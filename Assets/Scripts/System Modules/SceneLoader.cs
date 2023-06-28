using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneLoader : PersistenSingleton<SceneLoader>
{
    [SerializeField] UnityEngine.UI.Image transitionImage;
    [SerializeField] float fadeTime = 3.5f;
    Color color;

    static SceneInstance loadSceneInstance;
    public const string GAMEPLAY = "GamePlay";
    public const string MAIN_MENE = "MainMenu";
    public const string SCORING = "Scoring";

    public static event System.Action LoadingStarted;
    public static event System.Action<float> IsLoading;
    public static event System.Action LoadingSuccessed;
    public static event System.Action LoadingCompleted;

    public static bool ShowLoadingScreen {get; private set;}
    public static bool IsSceneLoaded {get; private set;}

    IEnumerator LoadingCoroutine(string sceneName)
    {
        // Load new scene in background and
        // Set this scene inactive
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        transitionImage.gameObject.SetActive(true);

        // Fade out
        while (color.a < 1f)
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;
        }

        // 优化 why TODO
        yield return new WaitUntil(()=> loadingOperation.progress >= 0.9f);

        // Activate the new scene
        loadingOperation.allowSceneActivation = true;

        // Fade in
        while (color.a > 0f)
        {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;
        }

        transitionImage.gameObject.SetActive(false);
    }

    public void LoadGamePlayScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GAMEPLAY));
    }

    public void LoadMainMenueScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MAIN_MENE));
    }

    public void LoadScoringScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(SCORING));
    }

    static IEnumerator LoadAddressableSceneCoroutine(object sceneKey, bool showLoadingScreen, bool laodSceneAdditively, bool activateOnLoad)
    {
        LoadSceneMode loadSceneMode = laodSceneAdditively ? LoadSceneMode.Additive : LoadSceneMode.Single;
        var asyncOperationHandle = Addressables.LoadSceneAsync(sceneKey, loadSceneMode, activateOnLoad);

        LoadingStarted?.Invoke();
        ShowLoadingScreen = showLoadingScreen;

        while (asyncOperationHandle.Status != AsyncOperationStatus.Succeeded)
        {
            IsLoading?.Invoke(asyncOperationHandle.PercentComplete);

            yield return null;
        }

        if (activateOnLoad)
        {
            LoadingCompleted?.Invoke();

            yield break;
        }

        LoadingSuccessed?.Invoke();
        IsSceneLoaded = true;
        loadSceneInstance = asyncOperationHandle.Result;
    }

    public static void ActivateLoadScene()
    {
        loadSceneInstance.ActivateAsync().completed += _ => 
        {
            IsSceneLoaded = false;
            loadSceneInstance = default;
            LoadingCompleted?.Invoke();
        };
    }

    public static void LoadAddressableScene(object sceneKey, bool showLoadingScreen = false, bool loadSceneAdditively = false, bool activateOnLoad = false)
    {
        Instance.StartCoroutine(LoadAddressableSceneCoroutine(sceneKey, showLoadingScreen, loadSceneAdditively, activateOnLoad));
    }
}
