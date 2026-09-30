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

    protected virtual void Movement()
    {
        transform.Translate(transform.up * (speed * Time.deltaTime));
    }
}
