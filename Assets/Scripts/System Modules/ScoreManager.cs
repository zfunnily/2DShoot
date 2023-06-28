using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistenSingleton<ScoreManager>
{
    public int Score => score;
    int score;
    int currentScore;
    Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);

    public void ResetScore()
    {
        score = 0;
        currentScore = 0;
        ScoreDisplay.UpdateText(score);
    }

    public void AddScore(int scorePoint)
    {
        currentScore += scorePoint;

        StartCoroutine(nameof(AddScoreCoroutine));
    }

    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);

        while (score < currentScore)
        {
            ScoreDisplay.UpdateText(++score);

            yield return null;
        }

        ScoreDisplay.ScaleText(Vector3.one);
    }
    
    [System.Serializable] public class PlayerScore
    {
        public int score;
        public string playerName;

        public PlayerScore(int score, string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }
    }

    [System.Serializable] public  class PlayerScoreData
    {
        public List<PlayerScore> list = new List<PlayerScore>();
    }

    readonly string SaveFileName = "player_socre.json";
    string playerName = "no name";

    public void SavePlayerScoreData()
    {
        var playerScoreData = LoadPlayerScoreData();
        playerScoreData.list.Add(new PlayerScore(score, playerName));
        playerScoreData.list.Sort((x,y) => y.score.CompareTo(x.score));

        SaveSystem.Save(SaveFileName, playerScoreData);
    }

    public PlayerScoreData LoadPlayerScoreData()
    {
        var playerScoreData = new PlayerScoreData();

        if (SaveSystem.SaveFileExists(SaveFileName))
        {
            playerScoreData = SaveSystem.Load<PlayerScoreData>(SaveFileName);
        }
        else
        {
            while (playerScoreData.list.Count < 10) playerScoreData.list.Add(new PlayerScore(0, playerName));

            SaveSystem.Save(SaveFileName, playerScoreData); 
        }

        return playerScoreData;
    }
}
