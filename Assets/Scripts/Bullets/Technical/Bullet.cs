using System;
using UnityEngine;

namespace Bullets
{
    public class Bullet : MonoBehaviour
    {
        private const float maxLifetime = 3f;
        private float lifetime = 0f;
    
        public Vector2 Velocity;

        [Header("Explosion Variables")] private bool willExplode = false;
        private float explodeTimer = 0f;
        private float timeToExplode;
        private int explosionBulletsCount;
        private float explosionSpeedValue;
        
        [Header("Homing Variables")] private bool willHoming = false;
        private GameObject homingTarget;
        private Vector3 bulletDirection;

        private void Start()
        {
            homingTarget = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (willHoming)
            {
                Vector3 bulletDirection = new Vector3(homingTarget.transform.position.x - transform.position.x, homingTarget.transform.position.y - transform.position.y, 0).normalized;
            }
            transform.position += (Vector3) Velocity * Time.deltaTime;
            lifetime += Time.deltaTime;

            if (lifetime > maxLifetime)
            {
                Disable();
            }

            if (willExplode)
            {
                explodeTimer += Time.deltaTime;
                if (explodeTimer >= timeToExplode)
                {
                    Explode();
                }
            }
           
        }

        private void Explode()
        {
            willExplode = false;
            ShotAttack.ExplosionShot(transform.position, explosionBulletsCount, explosionSpeedValue);
            gameObject.SetActive(false);
        }
        

        public void SetupExplosion(float time, int bullets, float speed)
        {
            willExplode = true;
            explodeTimer = 0f;
            timeToExplode = time;
            explosionBulletsCount = bullets;
            explosionSpeedValue = speed;

        }

        public void DisableExplosion()
        {
            willExplode = false;
            
        }
        
        private void Disable()
        {
            lifetime = 0f;
            gameObject.SetActive(false);
        }
        
    }
}
