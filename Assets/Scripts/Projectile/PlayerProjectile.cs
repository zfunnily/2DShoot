using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    TrailRenderer trail;

    void Awake() 
    {
        trail = GetComponentInChildren<TrailRenderer>();

        if (moveDirection != Vector2.right) Quaternion.FromToRotation(Vector2.right, moveDirection);
    }

    void OnDisable() 
    {
        trail.Clear();
    }
}
