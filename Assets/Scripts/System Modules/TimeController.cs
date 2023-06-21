using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0,1)] float bulletTimeScale = 0.1f;
    float defaultFixedDeltaTime;

    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }
    public void BulleTime(float duration)
    {
        Time.timeScale = bulletTimeScale;

        SlowOut(duration);
    }

    public void BulleTime(float inDuration, float outDuration)
    {
        SlowInAndOut(inDuration,outDuration);
    }

    public void BulleTime(float inDuration, float keepDuration, float outDuration)
    {
        SlowInAndOut(inDuration, keepDuration,outDuration);
    }

    public Coroutine SlowIn(float duration) => StartCoroutine(SlowInCoroutine(duration));
    public Coroutine SlowOut(float duration) => StartCoroutine(SlowOutCoroutine(duration));
    public Coroutine SlowInAndOut(float inDuration, float outDuration) => StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    public Coroutine SlowInAndOut(float inDuration, float keepingDuration, float outDuration) => StartCoroutine(SlowInKeepAndOutDuration(inDuration, keepingDuration, outDuration));

    IEnumerator SlowInKeepAndOutDuration(float inDuration, float keepingDuration, float outDuration)
    {
        yield return SlowIn(inDuration);
        yield return new WaitForSecondsRealtime(keepingDuration);

        SlowOut(outDuration);
    }

    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        yield return SlowIn(inDuration);

        SlowOut(outDuration);
    }

    IEnumerator SlowInCoroutine(float duration)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
            Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

            yield return null;
        }
    }

    IEnumerator SlowOutCoroutine(float duration)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
            Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

            yield return null;
        }
    }
}
