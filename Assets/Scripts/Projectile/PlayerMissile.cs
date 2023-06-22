using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayeProjectileOverdrive
{
    [Header("==== EXPLOSION ====")]
    [SerializeField] AudioData targetAcquireVoice = null;
    [SerializeField] AudioData explosionVoice = null;
    [SerializeField] GameObject explosionVFX = null;
    [SerializeField] LayerMask enemyLayerMask = default;
    [SerializeField] float explosionRadius = 3f;
    [SerializeField] float explosionDamage = 100f;


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

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        // Spawn a explosion VFX
        PoolManager.Release(explosionVFX, transform.position);
        // Play exposion SFX
        AudioManager.Instance.PlayRandomSFX(explosionVoice);
        // Enemies in explosion take AOE damage
        var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayerMask);
        // AOE 伤害
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(explosionDamage);
            }

        }
    }
}
