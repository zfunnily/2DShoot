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
}
