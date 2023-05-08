using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager :  Singleton<PoolManager>
{
    [SerializeField] Pool[] playerProjectilePools;

    void Start() 
    {
        Initialized(playerProjectilePools);
    }

    void Initialized(Pool[] pools) 
    {
        foreach (var pool in pools)
        {
            Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;

            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    public GameObject GetProjectil(Vector3 position, Quaternion rotation)
    {
        return playerProjectilePools[0].preparedObject(position, rotation);
    }
}
