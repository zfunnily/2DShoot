using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum GameState 
{
    Playing,
    Paused,
    GameOver,
}


public class GameManager : PersistenSingleton<GameManager>
{
    public static System.Action onGameOver;
    const string gameManager = "Game Manager";
    public static GameState GameState {get => Instance.gameState; set => Instance.gameState = value; }
    [SerializeField] GameState gameState = GameState.Playing;

    public static bool Playing() => GameState == GameState.Playing;
    public static bool Paused() => GameState == GameState.Paused;
    public static bool GameOver() => GameState == GameState.GameOver;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InstantiateGameManger()
    {
        // Runtime Assets loading
        // 1. Resources Folder
        // Resources.Load<GameObject>("GameManager"); out pass
        // 2. Addressable Assets
        Addressables.InstantiateAsync(gameManager).Completed += operationHandle => 
        {
            DontDestroyOnLoad(operationHandle.Result);
        } ;
    }

   }
