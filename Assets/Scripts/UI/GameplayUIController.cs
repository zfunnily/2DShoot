using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Canvas hUDCanvas;
    [SerializeField] Canvas menusCanvas;

    [Header("==== PLAYER INPUT ====")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button mainMenueButton;

    void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;

        resumeButton.onClick.AddListener(OnResumeButtonClick);
        optionsButton.onClick.AddListener(OnOptionsButtonClick);
        mainMenueButton.onClick.AddListener(OnMainMenueButtonClick);
    }

    void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;

        resumeButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();
        mainMenueButton.onClick.RemoveAllListeners();
    }

    void Pause()
    {
        Time.timeScale = 0f;
        hUDCanvas.enabled = false;
        menusCanvas.enabled = true;
        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();
    }

    void Unpause()
    {
        OnResumeButtonClick();
    }

    void OnResumeButtonClick()
    {
        Time.timeScale = 1f;
        hUDCanvas.enabled = true;
        menusCanvas.enabled = false;
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    void OnOptionsButtonClick()
    {
        // TODO
    }

    void OnMainMenueButtonClick()
    {
        menusCanvas.enabled = false;

        // load Main Menue Scene
        SceneLoader.Instance.LoadMainMenueScene();
    }

}
