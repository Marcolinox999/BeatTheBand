using System;
using System.Collections;
using UnityEngine;

namespace Bullets
{
    public class DualSpiral : MonoBehaviour
    {

        [Header("Settings")]
        [SerializeField] private float rotationSpeed = 12f;
        [SerializeField] private RadialShotSettings settings;

        private float angleClockwise = 0f;
        private float angleCounterClockwise = 180f;

        private bool isShooting = false;


        private void Update()
        {
            if (isShooting || settings == null)
                return;
            StartCoroutine(ShootDualSpiral());
        }

        private IEnumerator ShootDualSpiral()
        {
            isShooting = true;
            
            Vector2 origin = transform.position;
            Vector2 aimDirection = transform.up;
            
            ShotAttack.DualSpiral(origin, aimDirection,angleClockwise, angleCounterClockwise, settings);
            angleClockwise += rotationSpeed; 
            angleCounterClockwise -= rotationSpeed;


            yield return new WaitForSeconds(settings.cooldownTime);

            isShooting = false;
        }
    }
}

