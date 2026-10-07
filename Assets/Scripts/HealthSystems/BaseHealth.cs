using System;
using System.Collections;
using UnityEngine;

public abstract class BaseHealth : MonoBehaviour, IDamageable
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float damageCooldown;

    [SerializeField] protected Color _flashColor = Color.white;
    [SerializeField] protected float flashTime = 0.25f;
    protected SpriteRenderer[] _spriteRenderers;
    protected Material[]_materials;

    
    public float CurrentHealth { get; protected set; }
    public float MaxHealth => maxHealth;
    
    protected bool _canTakeDamage;


    protected virtual void Start()
    {
        CurrentHealth = maxHealth;
        _canTakeDamage = true;
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _materials = new Material[_spriteRenderers.Length];
        Init();
    }

    public virtual void ApplyDamage(float damage)
    {
        SpriteDamage();
        if (!_canTakeDamage || CurrentHealth <= 0f || damage <= 0f) return;
        CurrentHealth -= damage;
        
        if(CurrentHealth <= 0f)
        {
            CurrentHealth = 0f;
            Die();
        }
        else
        {
            StartCoroutine(DamageCooldown());
        }
        
    }

    #region DAMAGE FLASH

    private IEnumerator DamageFlasher()
    {
        //SET THE COLOR
        SetFlashColor();
        //LERP THE FLASH AMOUNT
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while(elapsedTime < flashTime)
        {
            //ITERATE ELAPSED TIME
            elapsedTime += Time.deltaTime;
            //LERP FLASH AMOUNT
            currentFlashAmount = Mathf.Lerp(1f, 0f, elapsedTime / flashTime);
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }

    }
    
    private void SetFlashAmount(float amount)
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat("_FlashAmount", amount);
        }
    }

    private void SetFlashColor()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor("_FlashColor", _flashColor);
        }
    }

    protected void CallDamageFlash()
    {
        StartCoroutine(DamageFlasher());
    }


    #endregion
    
    protected void Init()
    {
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }
    }
    
    
    private IEnumerator DamageCooldown()
    {
        _canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        _canTakeDamage= true;
    }

    protected abstract void SpriteDamage();
    protected abstract void Die();

}
