using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : PersistenSingleton<SceneLoader>
{
    [SerializeField] UnityEngine.UI.Image transitionImage;
    [SerializeField] float fadeTime = 3.5f;
    Color color;
    const string GAMEPLAY = "Gameplay";
    const string MAIN_MENE = "MainMenu";

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
}
