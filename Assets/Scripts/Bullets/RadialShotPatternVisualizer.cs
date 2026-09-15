using UnityEngine;

public class RadialShotPatternVisualizer : MonoBehaviour
{
    /*[SerializeField] private RadialShotSettings pattern;
    [SerializeField] private float radius;
    [SerializeField] private Color color;
    [SerializeField, Range (0f, 5f)] private float testTime;

    private void OnDrawGizmos()
    {
        if (pattern == null)
            return;
        
        Gizmos.color = color;
        
        int lap = 0;
        Vector2 aimDirection = transform.up;
        Vector2 center = transform.position;

        float timer = testTime;
        
        yield return new WaitForSeconds(pattern.startWait);
        while (timer > 0f && lap < pattern.repetitions)
        {
            for (int i = 0; i < pattern.patternSettings.Length; i++)
            {
                DrawRadialShot(pattern.patternSettings[i], timer, aimDirection);
                
                timer -= pattern.patternSettings[i].C
            }
        }
    }

    private void DrawRadialShot(RadialShotSettings settings, float lifetime, Vector2 aimDirection)
    {
        float angleBetweenBullets = 360f / settings.numberOfBullets;
        if (settings.phaseOffset != 0f || settings.angleOffset != 0f)
            aimDirection = aimDirection.Rotate((angleBetweenBullets * settings.angleOffset) + settings.angleOffset);
        
        for (int i = 0; i < settings.numberOfBullets; i++)
        {
            float bulletDirectionAngle = angleBetweenBullets * i;
            
            Vector2 bulletDirection = aimDirection.Rotate(bulletDirectionAngle);
            Vector2 bulletPosition = (Vector2)transform.position
                                     + bulletDirection * settings.bulletSpeed * lifetime;
            Gizmos.DrawSphere(bulletPosition, radius);
        }
    }*/
}