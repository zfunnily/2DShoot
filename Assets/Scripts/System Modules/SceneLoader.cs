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

    IEnumerator LoadCoroutine(string sceneName)
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
        StartCoroutine(LoadCoroutine(GAMEPLAY));
    }
}
