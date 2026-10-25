using System;
using System.Collections;
using Bullets;
using UnityEngine;

public abstract class BaseHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float damageCooldown;
        
        protected Color[] _originalColors;
        private Coroutine _flashCoroutine;

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
            if (!_canTakeDamage || CurrentHealth <= 0f || damage <= 0f) return;
            
            if (CompareTag("Player"))
            {
                ParticleDamage();
            }
            else
            {
                SpriteDamage();
            }
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

        public virtual void Heal(float amount)
        {
            if (!_canTakeDamage || CurrentHealth <= 0f || amount <= 0f) return;
            CurrentHealth += amount;
            if (CurrentHealth > maxHealth)
            {
                CurrentHealth = maxHealth;
            }
        }

        #region DAMAGE FLASH

        private IEnumerator DamageFlasher()
        {
         
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                _spriteRenderers[i].color = _flashColor; 
            }
        
            float elapsedTime = 0f;
            while(elapsedTime < flashTime)
            {
                elapsedTime += Time.deltaTime;
    

                for (int i = 0; i < _spriteRenderers.Length; i++)
                {
                    _spriteRenderers[i].color = Color.Lerp(_flashColor, _originalColors[i], elapsedTime / flashTime);
                }
            
                yield return null;
            }
            
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                _spriteRenderers[i].color = _originalColors[i];
            }
        }
        
        
        protected void CallDamageFlash()
        {
            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
            }
            
            _flashCoroutine = StartCoroutine(DamageFlasher());
        }


        #endregion
        
        protected void Init()
        {
            _originalColors = new Color[_spriteRenderers.Length];
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                _materials[i] = _spriteRenderers[i].material;
                _originalColors[i] = _spriteRenderers[i].color;
            }
        }
        
        
        private IEnumerator DamageCooldown()
        {
            _canTakeDamage = false;
            yield return new WaitForSeconds(damageCooldown);
            _canTakeDamage= true;
        }

        protected abstract void SpriteDamage();
        protected abstract void ParticleDamage();
        protected abstract void Die();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_canTakeDamage) return;
            
            if (CompareTag("Player"))
            {
                if (other.GetComponent<BaseBullet>() != null) return;
                var bullet = other.GetComponent<Bullet>();
                if (bullet != null)
                {
                    ApplyDamage(bullet.damage);
                    other.gameObject.SetActive(false);
                }
            }
            else if (CompareTag("Boss"))
            {
                if (other.GetComponent<Bullet>() != null) return;
                var bullet = other.GetComponent<BaseBullet>();
                if (bullet != null)
                {
                    ApplyDamage(bullet.damage);
                    other.gameObject.SetActive(false);
                }
            }
        }
    }
