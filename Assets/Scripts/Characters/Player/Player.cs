using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [SerializeField] bool regenerateHealth = true;
    [SerializeField] float  healthRegenerateTime;
    [SerializeField, Range(0, 1)] float  healthRegeneratePercent;

    [Header("---- INPUT ----")]
    [SerializeField] PlayerInput input;


    [Header("---- MOVE ----")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float accelerationTime = 3f;
    [SerializeField] float decelerationTime = 3f;
    [SerializeField] float moveRotationAngle = 50f;
    [SerializeField] float paddingX = .2f;
    [SerializeField] float paddingY = .2f;

    [Header("---- FIRE ----")]
    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleTop;
    [SerializeField] Transform muzzleBottom;
    [SerializeField, Range(0, 2)] int weaponPower = 0;
    [SerializeField] float fireInterval = 0.2f;
    WaitForSeconds waitForFireInterval;
    WaitForSeconds waithealthRegenerateTime;

    new Rigidbody2D rigidbody;

    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;

    public void Awake() 
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onWeaponChange += WeaponChange;
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

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waithealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);

        input.EnableGameplayInput();

        TakeDamage(50f);
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);

        Debug.Log("Regenerate health! Current Health: " + health + "\nTime: " + Time.time);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (gameObject.activeSelf)
        {
            if (regenerateHealth)
            {
                if (healthRegenerateCoroutine != null) StopCoroutine(healthRegenerateCoroutine);

                healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waithealthRegenerateTime, healthRegeneratePercent));
            }
        }
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

    void WeaponChange()
    {
        if (++weaponPower > 2) weaponPower = 0;
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            // Instantiate(projectile, muzzle.position, Quaternion.identity);
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(projectile1, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(projectile2, muzzleTop.position);
                    PoolManager.Release(projectile3, muzzleBottom.position);
                    break;
                case 2:
                    PoolManager.Release(projectile1, muzzleMiddle.position);
                    PoolManager.Release(projectile2, muzzleTop.position);
                    PoolManager.Release(projectile3, muzzleBottom.position);
                    break;
                default:
                    break;

            }

            yield return waitForFireInterval;
        }
    }
}
