using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("----- MOVE -----")]
    [SerializeField] float paddingX;
    [SerializeField] float paddingY;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float moveRotationAngle = 25f;

    [Header("----- FIRE -----")]
    [SerializeField] GameObject[] projectiles;
    [SerializeField] Transform muzzle;
    [SerializeField] float minFireInterval;
    [SerializeField] float maxFireInterval;

    void OnEnable() 
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    void OnDisable() 
    {
        StopAllCoroutines();
    }

    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        Vector3 targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            // if has not arrived targetPosition
            // keep moving to targetPosition
            // make enemy rotate with x axis while moving
            // else
            // set a new targetPosition

            if (Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else
            {
                targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }

            yield return null;
        }
    }

    IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
        }
    }
}
