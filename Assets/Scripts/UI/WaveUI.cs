using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    Text waveText;

    public void Awake() 
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        waveText = GetComponentInChildren<Text>();
    }

    public void OnEnable() 
    {
        waveText.text = "- WAVE " + EnemyManager.Instance.WaveNumber + " -";
    }
}
