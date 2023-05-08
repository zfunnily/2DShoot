using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput input;


    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float accelerationTime = 3f;
    [SerializeField] float decelerationTime = 3f;
    [SerializeField] float moveRotationAngle = 50f;
    [SerializeField] float paddingX = .2f;
    [SerializeField] float paddingY = .2f;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform muzzle;
    [SerializeField] float fireInterval = 0.2f;

    new Rigidbody2D rigidbody;

    Coroutine moveCoroutine;
    WaitForSeconds waitForSeconds;

    public void Awake() 
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
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

        waitForSeconds = new WaitForSeconds(fireInterval);

        input.EnableGameplayInput();
    }

    void Move(Vector2 moveInput)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);

        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveInput.normalized * moveSpeed, moveRotation));
        StartCoroutine(MovePositionLimitCoroutine());
    }

    // Update is called once per frame
    void StopMove()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
        StopCoroutine(MovePositionLimitCoroutine());
    }

    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.fixedDeltaTime / time;
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t / time);
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t/ time);

            yield return null;
        }
    }


    // 限制player在view point内
    IEnumerator MovePositionLimitCoroutine()
    {
        while(true)
        {
            this.transform.position = Viewport.Instance.PlayerMoveablePosition(this.transform.position, paddingX, paddingY);

            yield return null;
        }
    }

    
    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }

    void StopFire()
    {
        StopCoroutine(nameof(FireCoroutine));
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            PoolManager.Instance.GetProjectil(muzzle.position, Quaternion.identity);
            // Instantiate(projectile, muzzle.position, Quaternion.identity);

            yield return waitForSeconds;
        }
    }
}
