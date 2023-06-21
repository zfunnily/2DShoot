using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [Header("==== PLAYER INPUT ====")]
    [SerializeField] PlayerInput playerInput;

    [Header("==== CANVAS ====")]
    [SerializeField] Canvas hUDCanvas;
    [SerializeField] Canvas menusCanvas;

    [Header("==== AUDIO DATAT ====")]
    [SerializeField] AudioData pauseSFX;
    [SerializeField] AudioData unpauseSFX;

    [Header("==== BUTTONS ====")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button mainMenueButton;

    int buttonPressedParameterID = Animator.StringToHash("Pressed");

    void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;

        // 防止重复点击的问题
        ButtonPressedBehaviour.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClick);
        ButtonPressedBehaviour.buttonFunctionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehaviour.buttonFunctionTable.Add(mainMenueButton.gameObject.name, OnMainMenueButtonClick);
    }

    void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;
    }

    void Pause()
    {
        TimeController.Instance.Pause();
        GameManager.GameState = GameState.Paused;

        hUDCanvas.enabled = false;
        menusCanvas.enabled = true;
        playerInput.EnablePauseMenuInput();
        playerInput.SwitchToDynamicUpdateMode();

        UIInput.Instance.SelectUI(resumeButton);

        AudioManager.Instance.PlayerSFX(pauseSFX);
    }

    void Unpause()
    {
        resumeButton.Select();
        resumeButton.animator.SetTrigger(buttonPressedParameterID);

        AudioManager.Instance.PlayerSFX(unpauseSFX);
    }

    void OnResumeButtonClick()
    {
        TimeController.Instance.Unpause();
        GameManager.GameState = GameState.Playing;

        hUDCanvas.enabled = true;
        menusCanvas.enabled = false;
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    void OnOptionsButtonClick()
    {
        // TODO

        // 防止按下按钮时，游戏卡住
        UIInput.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuInput();
    }

    void OnMainMenueButtonClick()
    {
        menusCanvas.enabled = false;

        // load Main Menue Scene
        SceneLoader.Instance.LoadMainMenueScene();
    }

}
