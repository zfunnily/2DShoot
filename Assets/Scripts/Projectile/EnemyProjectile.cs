using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile 
{
    void Awake() 
    {
        if (moveDirection != Vector2.left) Quaternion.FromToRotation(Vector2.left, moveDirection);
    }
}
