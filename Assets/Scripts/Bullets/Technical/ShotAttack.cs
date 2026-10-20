using Unity.Collections;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

namespace Bullets
{
    public static class ShotAttack
    {
        public static Bullet SimpleShot(Vector2 origin, Vector2 velocity, RadialShotSettings settings= null)
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

            return bullet;
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
        public static void FlowerShot(Vector2 origin, Vector2 aimDirection, int numberOfBullets, float numberPetals,
            float baseSpeed, RadialShotSettings settings = null)
        {
            float angleSpeed = 360f/numberOfBullets;
            for (int i = 0;  i < numberOfBullets; i++)
            {
                float angle = i * angleSpeed;
                float rad =  Mathf.Deg2Rad * angle;
                
                float speedMultiplier = Mathf.Cos(numberPetals * rad);
                float currentSpeed = baseSpeed * (0.4f + 0.6f * Mathf.Abs(speedMultiplier));
                
                if(currentSpeed  < 0.5f) currentSpeed = 0.5f;  
                
                
                Vector2 bulletDirection = aimDirection.Rotate(angle);
                SimpleShot(origin, bulletDirection * currentSpeed, settings);
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

        public static void LineShotToPlayer(Vector2 origin, Vector2 targetPosition,
            int numberOfBullets,float spacing, float speed, RadialShotSettings settings = null)
        {
            Vector2 aimDirection = (targetPosition - origin).normalized;
            Vector2 rightDirection = aimDirection.Rotate(-90f).normalized;
            float totalWidth = (numberOfBullets - 1) * spacing;
            Vector2 startOrigin = origin - (rightDirection * (totalWidth / 2f));

            for (int i = 0; i < numberOfBullets; i++)
            {
                Vector2 spawnPosition = startOrigin + (rightDirection * (spacing * i));
                SimpleShot(spawnPosition, aimDirection * speed, settings);
            }
        }
        
        
        public enum WaterfallEdge{Top, Bottom, Left, Right}
        
        public static void PianoWaterfall(Vector2 origin, int numberOfBullets, float width, WaterfallEdge edge, float speed,  RadialShotSettings settings = null)
        {
            Vector2 spawnCenter = origin;
            Vector2 spawnDirection = Vector2.zero;
            Vector2 shootDirection = Vector2.down;

            Camera cam = Camera.main;
            if (cam != null)
            {
                float camHeight = 2f * cam.orthographicSize;
                float camWidth = camHeight * cam.aspect;
                Vector2 camPos = cam.transform.position;
                float outOfBoundsMargin = 2f;


                switch (edge)
                {
                    case WaterfallEdge.Top:
                        spawnCenter = new Vector2(camPos.x, camPos.y + (camHeight / 2f) + outOfBoundsMargin);
                        spawnDirection = Vector2.right;
                        shootDirection = Vector2.down;
                        break;
                    case WaterfallEdge.Bottom:
                        spawnCenter = new Vector2(camPos.x, camPos.y - (camHeight / 2f) - outOfBoundsMargin);
                        spawnDirection = Vector2.right;
                        shootDirection = Vector2.up;
                        break;
                    case WaterfallEdge.Left:
                        spawnCenter = new Vector2(camPos.x - (camWidth / 2f) - outOfBoundsMargin, camPos.y);
                        spawnDirection = Vector2.up;
                        shootDirection = Vector2.right;
                        break;
                    case WaterfallEdge.Right:
                        spawnCenter = new Vector2(camPos.x + (camWidth / 2f) + outOfBoundsMargin, camPos.y);
                        spawnDirection = Vector2.up;
                        shootDirection = Vector2.left;
                        break;
                }

                float spacing = numberOfBullets > 1 ? width / (numberOfBullets - 1) : 0;
                Vector2 startPos = spawnCenter - (spawnDirection * (width / 2f));
                for (int i = 0; i < numberOfBullets; i++)
                {
                    Vector2 spawnPosition = startPos + (spawnDirection * (spacing * i));
                    SimpleShot(spawnPosition, shootDirection * speed, settings);
                }
            }
        }

       

        public static void DualSpiral(Vector2 origin, Vector2 aimDirection, float angle1, float angle2,
            RadialShotSettings settings = null)
        {
            Vector2 dir1 = aimDirection.Rotate(angle1);
            RadialShot(origin, dir1, settings);
            
            Vector2 dir2 = aimDirection.Rotate(angle2);
            RadialShot(origin, dir2, settings);
        }

        public static void BoomerangRadial(Vector2 origin, Vector2 aimDirection, float returnForce,
            RadialShotSettings settings)
        {
            float angleBetweenBullets = 360f / settings.numberOfBullets;
            for (int i = 0; i < settings.numberOfBullets; i++)
            {
                Vector2 bulletDirection = aimDirection.Rotate(angleBetweenBullets * i).normalized;
                
                Bullet b = SimpleShot(origin, bulletDirection * settings.bulletSpeed, settings);
                
                b.SetupBoomerang(-bulletDirection * returnForce); 
            }
        }

        public static void StarBurst(Vector2 origin, Vector2 aimDirection, int points, int bulletsPerEdge, float speed,
            RadialShotSettings settings = null)
        {
            float angleBetweenPoints = 360f / points;
            float halfAngle = angleBetweenPoints / 2f;
            float innerRadius = 0.4f;
            
            for (int i = 0; i < points; i++)
            {
                float currentAngle = i * angleBetweenPoints;
                float nextAngle = currentAngle + halfAngle;
                float prevAngle = currentAngle - halfAngle;
                
                Vector2 outerTip = aimDirection.Rotate(currentAngle) * speed;
                Vector2 innerNext = aimDirection.Rotate(nextAngle) * (speed * innerRadius);
                Vector2 innerPrev = aimDirection.Rotate(prevAngle) * (speed * innerRadius);
                
                for (int j = 0; j < bulletsPerEdge; j++)
                {
                    float t = (float)j / bulletsPerEdge;
                    
                    Vector2 edgeVel1 = Vector2.Lerp(outerTip, innerNext, t);
                    SimpleShot(origin, edgeVel1, settings);

                    if (j > 0)
                    {
                        Vector2 edgeVel2 = Vector2.Lerp(outerTip, innerPrev, t);
                        SimpleShot(origin, edgeVel2, settings);
                    }
                }
            }
        }

        public static void StopAndGoRadial(Vector2 origin, Vector2 aimDirection, float timeToStop, float timeToGo,
            float goSpeed, RadialShotSettings settings)
        {
            float angleBetweenBullets = 360f / settings.numberOfBullets;
            for (int i = 0; i < settings.numberOfBullets; i++)
            {
                Vector2 bulletDirection = aimDirection.Rotate(angleBetweenBullets * i).normalized;
                
                Bullet b = SimpleShot(origin, bulletDirection * settings.bulletSpeed, settings);
                b.SetupStopAndGo(timeToStop, timeToGo, bulletDirection * goSpeed);
            }
        }

        public static void CageShot(Vector2 playerPosition, float radius, float gapAngle, RadialShotSettings settings)
        {
            float angleBetweenBullets = 360f / settings.numberOfBullets;
            Vector2 randomStartDir = Vector2.up.Rotate(Random.Range(0f, 360f));
            
            float timeToCenter = radius / settings.bulletSpeed;
            
            for (int i = 0; i < settings.numberOfBullets; i++)
            {
                float currentAngle = angleBetweenBullets * i;
                if (currentAngle < gapAngle) continue;

                Vector2 offset = randomStartDir.Rotate(currentAngle) * radius;
                Vector2 spawnPos = playerPosition + offset;
                
                Vector2 bulletDir = (playerPosition - spawnPos).normalized;

                Bullet b = SimpleShot(spawnPos, bulletDir * settings.bulletSpeed, settings);
                
                b.SetMaxLifetime(timeToCenter);
            }
        }
        
        public static void SpinningRadial(Vector2 origin, Vector2 aimDirection, float spinSpeed, RadialShotSettings settings)
        {
            float angleBetweenBullets = 360f / settings.numberOfBullets;

            for (int i = 0; i < settings.numberOfBullets; i++)
            {
                float bulletDirectionAngle = angleBetweenBullets * i;
                Vector2 bulletDirection = aimDirection.Rotate(bulletDirectionAngle).normalized;
                
                Bullet b = SimpleShot(origin, bulletDirection * settings.bulletSpeed, settings);
                b.SetupSpin(spinSpeed);
            }
        }
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
