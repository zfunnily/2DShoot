using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [SerializeField] bool destoryGameObject;
    [SerializeField] float lifetime = 3f;
    WaitForSeconds waitLifetime;


    void Awake() 
    {
        waitLifetime = new WaitForSeconds(lifetime);
    }

    void OnEnable() 
    {
        StartCoroutine(DeactivateCoroutine());
    }

    IEnumerator DeactivateCoroutine()
    {
        yield return waitLifetime;

        if (destoryGameObject) Destroy(gameObject);
        else gameObject.SetActive(false);
    }
}
