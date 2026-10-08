using System;
using UnityEngine;
using System.Collections;

namespace Bullets
{
    public class LineShotToPlayer : MonoBehaviour
    {
        [Header("Line Settings")]
        [SerializeField] private int numberOfBullets;
        [SerializeField] private float lineSpacing;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float cooldownTime;
        [SerializeField] private RadialShotSettings settings;

        private bool isLineShot = false;
        [SerializeField] Transform playerTransform;
        
        
        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        private void Update()
        {
            if (isLineShot || playerTransform == null)
                return;

            StartCoroutine(ShootLineRoutine());
        }

        private IEnumerator ShootLineRoutine()
        {
            isLineShot = true;
            Vector2 playerPos = playerTransform.position;
            Vector2 enemyPos = transform.position;
            
            ShotAttack.LineShotToPlayer(enemyPos, playerPos, numberOfBullets, lineSpacing, bulletSpeed, settings);
            
            yield return new WaitForSeconds(cooldownTime);
            isLineShot = false;
        }
    }
}
    
