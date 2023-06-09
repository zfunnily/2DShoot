using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.Initialization;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.AddressableAssets.ResourceProviders;

public class ScoringUIController : MonoBehaviour
{
    [Header("==== BACKGROUND ====")]
    [SerializeField] public AssetLabelReference SpaceBackgroundLable;
    [SerializeField] Image background;
    Dictionary<string,Sprite> images;
    Sprite[] backgroundImages;

    [Header("==== SCORING SCREEN ====")]
    [SerializeField] Canvas scoringScreenCanvas;
    [SerializeField] Text playerScoreText;
    [SerializeField] Button buttonMainMenue;
    [SerializeField] Transform highScoreLeaderboardContainer;

    void Start()
    {
       images = new Dictionary<string, Sprite>(); 
       Addressables.LoadAssetsAsync<Sprite>(SpaceBackgroundLable, (obj) => 
       {
            images.Add(obj.name, obj);
       }).Completed += (obj) => 
       {
            backgroundImages = new Sprite[images.Count];

            var i = 0;
            foreach (var image in images) 
            {
                    backgroundImages[i] = image.Value;
                    i++;
            }

            ShowRandomBackground();
       };

       ShowScoringScreen();

       ButtonPressedBehaviour.buttonFunctionTable.Add(buttonMainMenue.gameObject.name, OnButtonMainMenueClick);
       GameManager.GameState = GameState.Scoring;
    }

    void ShowRandomBackground()
    {
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
    }

    void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(buttonMainMenue);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        UpdateHighScoreLeaderBoard();

    }

    void UpdateHighScoreLeaderBoard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;

        for (int i = 0; i < highScoreLeaderboardContainer.childCount; i++) 
        {
            var child = highScoreLeaderboardContainer.GetChild(i);

            child.Find("Rank").GetComponent<Text>().text = (i+1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName;
        }
    }

    void OnButtonMainMenueClick() 
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenueScene();
    }
}
