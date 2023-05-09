using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager :  MonoBehaviour
{
    [SerializeField] Pool[] playerProjectilePools;
    [SerializeField] Pool[] enemyProjectilePools;

    static Dictionary<GameObject, Pool> dictionary;

    void Start() 
    {
        dictionary = new Dictionary<GameObject, Pool>();

        Initialized(playerProjectilePools);
        Initialized(enemyProjectilePools);
    }

    #if UNITY_EDITOR
    void OnDestroy() {
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
    }
    #endif

    void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(
                    string.Format("Pool: {0} has a runtime size {1} bigger than its initial size {2}!",
                    pool.Prefab.name,
                    pool.RuntimeSize,
                    pool.Size)
                );
            }
        }
    }

    void Initialized(Pool[] pools) 
    {
        foreach (var pool in pools)
        {
            #if UNITY_EDITOR
                if (dictionary.ContainsKey(pool.Prefab)) 
                {
                    Debug.LogError("Same prefab in multiple pool! Prefab: " + pool.Prefab.name);
                    continue;
                }
            #endif

            dictionary.Add(pool.Prefab, pool);

            Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;

            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }


    public static GameObject Release(GameObject prefab)
    {
        return dictionary[prefab].PreparedObject();
    }

    public static GameObject Release(GameObject prefab, Vector3 position)
    {
        return dictionary[prefab].PreparedObject(position);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return dictionary[prefab].PreparedObject(position, rotation);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        return dictionary[prefab].PreparedObject(position, rotation, localScale);
    }
}
