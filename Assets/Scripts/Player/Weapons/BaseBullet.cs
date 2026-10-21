using System;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] protected float lifetime;

    private float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
        Movement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boss")
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
                
            if (damageable != null)
            {
                damageable.ApplyDamage(damage);
            }
            Destroy(gameObject);
            
        }
    }
    protected virtual void Movement()
    {
        transform.Translate(-transform.right * (speed * Time.deltaTime),  Space.World);
    }
}
