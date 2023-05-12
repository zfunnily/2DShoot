using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("---- DEATH ----")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioData[] deathSFX;

    [Header("---- HEALTH ----")]
    [SerializeField] protected float maxHealth;
    [SerializeField] bool showOnHeadHealthBar = true;
    [SerializeField] StateBar onHeadHealthBar;

    protected float health;
    protected virtual void OnEnable() 
    {
        health = maxHealth;

        if (showOnHeadHealthBar) ShowOnHeadHealthBar();
        else HideOnHeadHealthBar();
    }

    public void ShowOnHeadHealthBar()
    {
        if (onHeadHealthBar == null) return;

        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxHealth);
    }

    public void HideOnHeadHealthBar()
    {
       if (onHeadHealthBar != null) onHeadHealthBar.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (showOnHeadHealthBar && gameObject.activeSelf) onHeadHealthBar.UpdateStats(health, maxHealth);

        if (health <= 0f) Die();
    }

    public virtual void Die()
    {
        health = 0f;
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        PoolManager.Release(deathVFX, transform.position);
        gameObject.SetActive(false);
    }

    public virtual void RestoreHealth(float value)
    {
        if (health == maxHealth) return;

        // health += value;
        // health = Mathf.Clamp(health, 0f, maxHealth);
        health = Mathf.Clamp(health + value, 0f, maxHealth);

        if (showOnHeadHealthBar) onHeadHealthBar.UpdateStats(health, maxHealth);
    }

    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health < maxHealth)
        {
            yield return waitTime;

            RestoreHealth(maxHealth * percent);
        }
    }

    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health > 0f)
        {
            yield return waitTime;
            TakeDamage(maxHealth * percent);
        }
    }
}
