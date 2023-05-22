using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    [SerializeField] StatsBar_HUB statsBar_HUB;
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
    [SerializeField] float fireInterval = 0.12f;
    [SerializeField] AudioData projectileLaunchSFX;

    [Header("---- DODGE ----")]
    [SerializeField] AudioData dodgeSFX;
    [SerializeField, Range(0,100)] int dodgeEnergyCost = 25;
    [SerializeField] float maxRoll = 720f;
    [SerializeField] float rollSpeed = 360f; // 
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);

    [Header("---- OVERDRIVE ----")]
    [SerializeField] int overdriveDodgeFactor = 2;
    [SerializeField] float overdriveSpeedFactor = 1.2f;
    [SerializeField] float overdriveFireFactor = 1.2f;
    bool isOverdriving = false;
    
    bool isDodging = false;
    float dodgeDuration;
    float currentRoll = 0f;

    float t = 0f;
    Vector2 previousVelocity; 
    Quaternion previousRotation;
    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitForOverdriveFireInterval;
    WaitForSeconds waithealthRegenerateTime;
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate(); 
    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;
    new Rigidbody2D rigidbody;
    new Collider2D collider;

    public void Awake() 
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        dodgeDuration = maxRoll / rollSpeed;

        rigidbody.gravityScale = 0f;

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval / overdriveFireFactor);
        waithealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onWeaponChange += WeaponChange;
        input.onDodge += Dodge;
        input.onOverdrive += Overdrive;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }

    void OnDisable() 
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onWeaponChange -= WeaponChange;
        input.onDodge -= Dodge;
        input.onOverdrive -= Overdrive;

        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }

    // Start is called before the first frame update
    void Start () 
    {
        

        statsBar_HUB.Initialize(health, maxHealth);

        input.EnableGameplayInput();
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        statsBar_HUB.UpdateStats(health, maxHealth);

        Debug.Log("Regenerate health! Current Health: " + health + "\nTime: " + Time.time);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        statsBar_HUB.UpdateStats(health, maxHealth);

        if (gameObject.activeSelf)
        {
            if (regenerateHealth)
            {
                if (healthRegenerateCoroutine != null) StopCoroutine(healthRegenerateCoroutine);

                healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waithealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }

    public override void Die()
    {
        statsBar_HUB.UpdateStats(0f, maxHealth);

        base.Die();
    }


    void Move(Vector2 moveInput)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);

        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveInput.normalized * moveSpeed, moveRotation));

        StartCoroutine(nameof(MovePositionLimitCoroutine));
    }

    // Update is called once per frame
    void StopMove()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));

        StopCoroutine(nameof(MovePositionLimitCoroutine));
    }

    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        t = 0f;
        previousVelocity = rigidbody.velocity;
        previousRotation = transform.rotation;

        while (t < 1f)
        {
            t += Time.fixedDeltaTime / time;

            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t);

            yield return waitForFixedUpdate;
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

            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);

            yield return isOverdriving?waitForOverdriveFireInterval:waitForFireInterval;

        }
    }
    
    void Dodge()
    {
        if (isDodging || !PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;

        StartCoroutine(nameof(DodgeCoroutine));
        
    }

    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        AudioManager.Instance.PlayerSFX(dodgeSFX);
        // 1. Cost energy 
        // 2. Make player invincible 让玩家无敌
        // 3. Make player rotate alone x axis 让玩家沿着x轴旋转
        // 4. Change player's scale 改变玩家的缩放值

        PlayerEnergy.Instance.Use(dodgeEnergyCost);

        // 2 start
        collider.isTrigger = true;

        // 3
        currentRoll = 0f;

        // var scale = transform.localScale;
        // var t1 = 0f;
        // var t2 = 0f;

        // while (currentRoll < maxRoll)
        // {
            // currentRoll += rollSpeed * Time.deltaTime;
            // transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

            // if (currentRoll < maxRoll / 2f)
            // {
                // 方法一
                // scale -= (Time.deltaTime / dodgeDuration) * Vector3.one;

                // 方法二
                // scale.x = Mathf.Clamp(scale.x - Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
                // scale.y = Mathf.Clamp(scale.y - Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
                // scale.z = Mathf.Clamp(scale.z - Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);

                // 方法三
                // t1 += Time.deltaTime / dodgeDuration;
                // transform.localScale = Vector3.Lerp(transform.localScale, dodgeScale, t1);

            // }
            // else
            // {
                // scale += (Time.deltaTime / dodgeDuration) * Vector3.one;

                // scale.x = Mathf.Clamp(scale.x + Time.deltaTime / dodgeDuration, dodgeScale.x, 1f);
                // scale.y = Mathf.Clamp(scale.y + Time.deltaTime / dodgeDuration, dodgeScale.y, 1f);
                // scale.z = Mathf.Clamp(scale.z + Time.deltaTime / dodgeDuration, dodgeScale.z, 1f);

                // t2 += Time.deltaTime / dodgeDuration;
                // transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, t2);
            // }

            // yield return null;
        // }

        // 方法四
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);

            yield return null;
        }

        
        // 2 end
        collider.isTrigger = false;
        isDodging = false;
    }

        
    void Overdrive()
    {
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;

        PlayerOverdrive.on.Invoke();
    }

    void OverdriveOn()
    {
        isOverdriving = true;
        dodgeEnergyCost *= overdriveDodgeFactor;
        moveSpeed *= overdriveSpeedFactor;
    }
    void OverdriveOff()
    {
        isOverdriving = false;
        dodgeEnergyCost /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
    }
}
