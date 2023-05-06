using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput input;


    [SerializeField]float moveSpeed = 10f;
    [SerializeField]float paddingX= .2f;
    [SerializeField]float paddingY= .2f;

    new Rigidbody2D rigidbody;

    public void Awake() 
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove+= StopMove;
    }

    void OnDisable() 
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
    }

    // Start is called before the first frame update
    void Start () 
    {
        rigidbody.gravityScale = 0f;
        input.EnableGameplayInput();
    }

    void Move(Vector2 moveInput)
    {
        rigidbody.velocity = moveInput * moveSpeed;
        StartCoroutine(MovePositionLimitCoroutine());
    }

    // Update is called once per frame
    void StopMove()
    {
        rigidbody.velocity = Vector2.zero;
        StopCoroutine(MovePositionLimitCoroutine());
    }

    IEnumerator MovePositionLimitCoroutine()
    {
        while(true)
        {
            this.transform.position = Viewport.Instance.PlayerMoveablePosition(this.transform.position, paddingX, paddingY);

            yield return null;
        }
    }
}
