using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateBar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;
    [SerializeField] bool delayFill = true;
    [SerializeField] float fillDelay = 0.5f;
    [SerializeField] float fillSpeed = 0.1f;

    float currentFillAmount;
    protected float targetFillAmount;
    float t;
    WaitForSeconds waitForDelayFill;
    Coroutine bufferedFillingCoroutine;
    Canvas canvas;

    void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas)) canvas.worldCamera = Camera.main; 

        waitForDelayFill = new WaitForSeconds(fillDelay);
    }

    public void OnDisable() 
    {
        StopAllCoroutines();
    }

    public virtual void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;
    }

    public void UpdateStats(float currentValue, float maxValue)
    {
        targetFillAmount = currentValue / maxValue;

        if (bufferedFillingCoroutine != null) StopCoroutine(bufferedFillingCoroutine);

        // if stats reduce
        // fill image front's fill amount = target fill amount
        // slowly reduce fill image back's fill amount

        if (currentFillAmount > targetFillAmount)
        {
            fillImageFront.fillAmount = targetFillAmount;
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));
            return;
        }

        // if stats increase 
        // fill image back's fill amount = target fill amount
        // slowly increase fill image front's fill amount

        if (currentFillAmount < targetFillAmount)
        {
            fillImageBack.fillAmount = targetFillAmount;
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }

    }

    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (delayFill)
        {
            yield return waitForDelayFill;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;

            yield return null;
        }
    }
}
