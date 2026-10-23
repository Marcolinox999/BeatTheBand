using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class MusicalReward : BaseHealth
{
    [Header("Movement")]
    [SerializeField] private float speed = 15f;
    [Tooltip("Max random angle (degrees) added to the reflected direction on each bounce.")]
    [SerializeField] private float maxBounceAngle = 25f;

    [Header("Damage")]
    [SerializeField] private float damagePerBullet = 1f;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 6f;
    [Tooltip("How fast the ball returns to its normal speed after a knockback.")]
    [SerializeField] private float speedRecovery = 20f;

    private float normalScale;
    [SerializeField] private float bouncyScale;
    private bool scaling;
    private bool knockbacking;
    [Header("Audio")]
    [SerializeField] private AudioClip[] bounceSounds;

    [Header("Destruction")]
    [SerializeField] private int levelCompleted;
    [SerializeField] private int weaponUnlock;
    [SerializeField] private float destroyDelay = 1.5f;

    [Header("Level Change")] 
    [SerializeField] private string mapSceneName;

    private Rigidbody2D rb;
    private Vector2 lastVelocity;
    private bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        normalScale = rb.transform.localScale.x;
    }

    protected override void Start()
    {
        base.Start();
        Launch(RandomDirection());
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        
        if (rb.linearVelocity.sqrMagnitude < 0.0001f)
        {
            Launch(RandomDirection());
        }

        float currentSpeed = rb.linearVelocity.magnitude;
        if (!Mathf.Approximately(currentSpeed, speed))
        {
            float newSpeed = Mathf.MoveTowards(currentSpeed, speed, speedRecovery * Time.fixedDeltaTime);
            rb.linearVelocity = rb.linearVelocity.normalized * newSpeed;
        }

        lastVelocity = rb.linearVelocity;
    }

    public void Launch(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
    }

    private Vector2 RandomDirection()
    {
        return Random.insideUnitCircle.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        knockbacking = true;
        Vector2 normal = collision.GetContact(0).normal;
        Vector2 reflected = Vector2.Reflect(lastVelocity, normal);

        float randomAngle = Random.Range(-maxBounceAngle, maxBounceAngle);
        Vector2 newDirection = Quaternion.Euler(0f, 0f, randomAngle) * (Vector3)reflected;

        rb.linearVelocity = newDirection.normalized * speed;

        PlayRandomBounceSound();
    }

    private void PlayRandomBounceSound()
    {
        if (bounceSounds == null || bounceSounds.Length == 0) return;

        AudioClip clip = bounceSounds[Random.Range(0, bounceSounds.Length)];
        AudioManager.instance.PlaySFX(clip);
    }
    
/*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead || !other.CompareTag("Bullet")) return;

        ApplyKnockback(GetBulletDirection(other));
        ApplyDamage(damagePerBullet);
    }
*/ 
    private Vector2 GetBulletDirection(Collider2D bullet)
    {
        if (bullet.attachedRigidbody != null && bullet.attachedRigidbody.linearVelocity.sqrMagnitude > 0.01f)
            return bullet.attachedRigidbody.linearVelocity.normalized;

        return (transform.position - bullet.transform.position).normalized;
    }

    private void ApplyKnockback(Vector2 direction)
    {
        rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
    }

    protected override void SpriteDamage()
    {
        CallDamageFlash();
    }

    protected override void ParticleDamage()
    {
        // Only called for objects tagged "Player", so unused here
    }

    protected override void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(DestructionSequence());
    }
    

    private IEnumerator DestructionSequence()
    {
        GetComponent<Collider2D>().enabled = false;
        if (GameManager.Instance != null)
        {
            if (levelCompleted >= 0) GameManager.Instance.CompleteLevel(levelCompleted);
            if (weaponUnlock >= 0) GameManager.Instance.UnlockWeapon(weaponUnlock);
        }
        
        yield return new WaitForSeconds(destroyDelay);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReturnToMap(mapSceneName);
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        if (!knockbacking) return;
        if (scaling)
        {
            float actualScale = math.lerp(transform.localScale.x, bouncyScale, 7 * Time.fixedDeltaTime);
            transform.localScale = new Vector3(actualScale, actualScale, actualScale);
        }
        else
        {
            float actualScale = math.lerp(transform.localScale.x, normalScale, 7 * Time.fixedDeltaTime);
            transform.localScale = new Vector3(actualScale, actualScale, actualScale);
        }

        if (transform.localScale.x - bouncyScale <= 0.001f)
        {
            scaling = false;
        }
        else if (normalScale -transform.localScale.x <= 0.001f)
        {
            scaling = true;
            knockbacking = false;
        }
    }
}