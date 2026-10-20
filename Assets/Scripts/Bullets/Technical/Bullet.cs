using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bullets
{
    public class Bullet : MonoBehaviour
    {
        private float currentMaxLifetime = 8f;
        private float lifetime = 0f;
    
        public Vector2 Velocity;

        [Header("Rotation Variables")] private bool isSpinning = false;
        private float spinSpeed = 0f;
        
        
        [Header("Explosion Variables")] private bool willExplode = false;
        private float explodeTimer = 0f;
        private float timeToExplode;
        private int explosionBulletsCount;
        private float explosionSpeedValue;
        
        [Header("Boomerang Variables")] private bool isBoomerang = false;
        private Vector2 acceleration;
        
        [Header("Stop & Go Variables")]  private bool isStopAndGo = false;
        private float stopTimer = 0f;
        private float timeToStop = 0f;
        private float timeToGo = 0f;
        private Vector2 goVelocity;
        private bool hasStopped = false;
        private bool hasGone = false;
        
        
        private void OnEnable()
        {
            currentMaxLifetime = 15f;
            lifetime = 0f;
            
            isSpinning = false;
            spinSpeed = 0f;
            
            willExplode = false;
            explodeTimer = 0f;
            
            isBoomerang = false;
            acceleration = Vector2.zero;
            
            isStopAndGo = false;
            stopTimer = 0f;
            
            isSpinning = false;
            spinSpeed = 0f;
            
            hasStopped = false;
            hasGone = false;
        }


        private void Update()
        {
            if (lifetime > currentMaxLifetime) Disable();
            if (isBoomerang)
            {
                Velocity += acceleration * Time.deltaTime;
            }

            if (isStopAndGo && !hasGone)
            {
                stopTimer += Time.deltaTime;

                if (!hasStopped && stopTimer >= timeToStop)
                {
                    Velocity = Vector2.zero; // Frenada en seco
                    hasStopped = true;
                }
                else if (hasStopped && stopTimer >= (timeToStop + timeToGo))
                {
                    Velocity = goVelocity; // Arranca de nuevo
                    hasGone = true;
                }
            }

            if (isSpinning)
            {
                Velocity = Velocity.Rotate(spinSpeed * Time.deltaTime);
                spinSpeed = Mathf.Lerp(spinSpeed, 0f, Time.deltaTime * 1.5f);
            }

            transform.position += (Vector3)Velocity * Time.deltaTime;
            lifetime += Time.deltaTime;

            if (lifetime > currentMaxLifetime)
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



        #region BULLET CONFIGURATIONS
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
        
        public void SetupSpin(float speed)
        {
            isSpinning = true;
            spinSpeed = speed;
        }

        public void SetupBoomerang(Vector2 accelVector)
        {
            isBoomerang = true;
            acceleration = accelVector;
        }
        
        public void SetupStopAndGo(float stopDelay, float waitDelay, Vector2 newVelocity)
        {
            isStopAndGo = true;
            timeToStop = stopDelay;
            timeToGo = waitDelay;
            goVelocity = newVelocity;
        }
        
        
        #endregion
        
        public void SetMaxLifetime(float time)
        {
            currentMaxLifetime = time;
        }
        private void Disable()
        {
            gameObject.SetActive(false);
        }
        
   
        
        
    }
}
