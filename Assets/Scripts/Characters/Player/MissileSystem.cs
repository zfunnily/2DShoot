using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab = null;
    [SerializeField] AudioData launchSFX = null;

    public void Launch(Transform muzzleTransform)
    {
        PoolManager.Release(missilePrefab, muzzleTransform.position);
        AudioManager.Instance.PlayRandomSFX(launchSFX);
    }
}
