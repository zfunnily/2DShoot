using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayeProjectileOverdrive
{
    [SerializeField] AudioData targetAcquireVoice = null;
    [SerializeField] AudioData explosionVoice = null;

    [Header("==== SPEED CHANGE ====")]
    [SerializeField] float lowSpeed = 8f;
    [SerializeField] float highSpeed = 35f;
    [SerializeField] float variableSpeedDelay = 0.5f;

    WaitForSeconds waitVariableSpeedDelay;

    protected override void Awake() 
    {
        base.Awake();

        waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }


    IEnumerator VariableSpeedCoroutine()
    {
        moveSpeed = lowSpeed;

        yield return waitVariableSpeedDelay;

        moveSpeed = highSpeed;

        if (target != null)
        {
            AudioManager.Instance.PlayRandomSFX(targetAcquireVoice);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        AudioManager.Instance.PlayRandomSFX(explosionVoice);
    }
}
