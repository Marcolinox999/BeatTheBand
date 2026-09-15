using UnityEngine;

public static class ShotAttack
{
    public static void SimpleShot(Vector2 origin, Vector2 velocity)
    {
        Bullet bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = origin;
        bullet.Velocity = velocity;
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
            SimpleShot(origin, bulletDirection * settings.bulletSpeed);
        }
    }
}
