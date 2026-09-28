using System;
using System.Collections;
using UnityEngine;

public abstract class BaseHealth : MonoBehaviour, IDamageable
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float damageCooldown;
    
    public float CurrentHealth { get; protected set; }
    public float MaxHealth => maxHealth;
    
    protected bool _canTakeDamage;

    protected virtual void Start()
    {
        CurrentHealth = maxHealth;
        _canTakeDamage = true;
    }

    public void ApplyDamage(float damage)
    {
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
    
    
    private IEnumerator DamageCooldown()
    {
        _canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        _canTakeDamage= true;
    }

    protected abstract void Die();

}
