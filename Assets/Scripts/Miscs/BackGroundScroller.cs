using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    [SerializeField] Vector2 scrollVelocity;
    Material material;

    void Awake() {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    IEnumerator Start()
    {
        while(!GameManager.GameOver())
        {
            material.mainTextureOffset += scrollVelocity * Time.deltaTime;

            yield return null;
        }
    }

}
