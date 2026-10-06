using System.Collections;
using UnityEngine;

namespace Bullets
{
    public class FlowerShot : MonoBehaviour
    {
        [Header("FlowerSettings")]
        [SerializeField] private int numberOfBullets;
        [SerializeField] private int numberOfPetals;
        [SerializeField] private float baseSpeed;
        [SerializeField] private float speedVar;
        [SerializeField] private float cooldownTime;
        [SerializeField] private RadialShotSettings settings;

        private bool isFlower;
        
        private void Update()
        {
            if (isFlower) return;

            StartCoroutine(ShootFlowerRoutine());
        }

        private IEnumerator ShootFlowerRoutine()
        {
            isFlower = true;
            
            Vector2 origin = transform.position;
            Vector2 aimDirection = transform.up;
            
            ShotAttack.FlowerShot(origin, aimDirection, numberOfBullets, numberOfPetals, baseSpeed, speedVar, settings);
            yield return new WaitForSeconds(cooldownTime);
            
            isFlower = false;
        }

    }
}

