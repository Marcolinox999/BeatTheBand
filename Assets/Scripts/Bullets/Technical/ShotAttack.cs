using Unity.Collections;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

namespace Bullets
{
    public static class ShotAttack
    {
        public static void SimpleShot(Vector2 origin, Vector2 velocity, RadialShotSettings settings= null)
        {
            Bullet bullet = BulletPool.Instance.GetBullet();
            bullet.transform.position = origin;
            bullet.Velocity = velocity;
            
            if (settings != null && settings.doesExplode)
            {
                bullet.SetupExplosion(settings.timeToExplode, settings.explosionBullets, settings.explosionSpeed);
            }
            else
            {
                bullet.DisableExplosion();
            }
        }

        public static void RadialShot
            (Vector2 origin, Vector2 aimDirection, RadialShotSettings settings)
        {
            float angleBetweenBullets = 360f / settings.numberOfBullets;

            if(settings.angleOffset != 0f || settings.phaseOffset != 0f)
                aimDirection = aimDirection.Rotate(settings.angleOffset + (settings.phaseOffset * angleBetweenBullets));
        
            for (int i = 0; i < settings.numberOfBullets; i++)
            {
                float bulletDirectionAngle = angleBetweenBullets * i;

                if (settings.radialMask && bulletDirectionAngle > settings.maskAngle)
                    break;
            
                Vector2 bulletDirection = aimDirection.Rotate(bulletDirectionAngle);
                SimpleShot(origin, bulletDirection * settings.bulletSpeed, settings);
            }
        }

        public static void ParallelShot(Vector2 center, Vector2 aimDirection, int numberLines, float spacing, float speed)
        {
            Vector2 rightDirection = aimDirection.Rotate(-90f).normalized;
            float totalWidth = (numberLines - 1) * spacing;
            Vector2 startOrigin = center - (rightDirection * (totalWidth / 2f));
            for (int i = 0; i < numberLines; i++)
            {
                Vector2 spawnPosition = startOrigin + (rightDirection * (spacing * i));
                SimpleShot(spawnPosition, aimDirection * speed);
            }
        }

        public static void ExplosionShot(Vector2 origin, int numberOfBullets, float speed)
        {
            float angleBetweenBullets = 360f / numberOfBullets;
            Vector2 aimDirection = Vector2.up;
            for (int i = 0; i < numberOfBullets; i++)
            {
                Vector2 bulletDirection = aimDirection.Rotate(angleBetweenBullets * i);
                SimpleShot(origin, bulletDirection * speed, null);
            }
        }
        public static void ConeShotToPlayer(Vector2 origin, Vector2 targetPosition, RadialShotSettings settings, int numberOfBullets, float coneAngle, float speed)
        {
            Vector2 aimDirection = (targetPosition - origin).normalized;
            if (numberOfBullets == 1)
            {
                SimpleShot(origin, aimDirection * speed,settings);
            }
            
            float halfCone = coneAngle * 0.5f;
            float angleStep = coneAngle / (numberOfBullets - 1);
            for (int i = 0; i < numberOfBullets; i++)
            {
                float currentAngleOffset = -halfCone + (angleStep * i);
                Vector2 bulletDirection = aimDirection.Rotate(currentAngleOffset);
                SimpleShot(origin, bulletDirection * speed, null);
            }
        }
        
        
        /* // Ejemplo de llamada dentro de la lógica de un enemigo
Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

ShotAttack.ConeShotToPlayer(
    transform.position,    // Origen (dónde está el enemigo)
    playerPos,             // Posición actual del jugador
    myRadialSettings,      // Tus ajustes de explosión / configuración
    5,                     // Número de balas en el cono
    60f,                   // Apertura del cono en grados (ej: 60 grados de abanico)
    bulletSpeed            // Velocidad de las balas
);*/
        #region WeaponAttacks

        public static void TrumpetShot(Vector2 position, Vector2 direction, float speed)
        {
            Bullet bullet = BulletPool.Instance.GetBullet(); 
            bullet.transform.position = position;
            bullet.Velocity = direction.normalized * speed;
        }

        #endregion
    }
}
