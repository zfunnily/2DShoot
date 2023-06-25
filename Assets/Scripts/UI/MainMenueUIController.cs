using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenueUIController : MonoBehaviour
{
    [Header("==== CANVAS ====")]
    [SerializeField] Canvas mainMenueCanvas;

    [Header("==== BUTTONS ====")]
    [SerializeField] Button buttonStart;
    [SerializeField] Button buttonOptions;
    [SerializeField] Button buttonQuit;

    void OnEnable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonStart.gameObject.name, OnButtonStartClick);
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonOptions.gameObject.name, OnButtonOptionsClick);
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonQuit.gameObject.name, OnButtonQuitClick);
    }

    void OnDisable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Clear();
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }

    void OnButtonStartClick()
    {
        mainMenueCanvas.enabled = false;
        SceneLoader.Instance.LoadGamePlayScene();
    }

    void OnButtonOptionsClick()
    {
        UIInput.Instance.SelectUI(buttonOptions);
    }

    void OnButtonQuitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); // 只在编译好的程序才能执行. 在unity程序中是不生效的.
        #endif
    }
}
