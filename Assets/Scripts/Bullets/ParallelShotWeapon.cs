using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Bullets
{
    public class ParallelShotWeapon : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private int numberLines;
        [SerializeField] private float lineSpacing;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float timeBetweenWaves;

        private bool isShooting = false;
        
        void Update()
        {
            if (!isShooting)
                StartCoroutine(ShootPentagram());
        }

       private IEnumerator ShootPentagram()
       {
           isShooting = true;
           Vector2 aimDirection = transform.up;
           ShotAttack.ParallelShot(transform.position, aimDirection, numberLines, lineSpacing, bulletSpeed);
           yield return new WaitForSeconds(timeBetweenWaves);
           isShooting = false;
       }
    }
}