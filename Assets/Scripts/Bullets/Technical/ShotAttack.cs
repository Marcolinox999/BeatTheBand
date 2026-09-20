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
            
            //esto es para que cuando recicle balas no exploten
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
    }
}
