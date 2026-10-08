/*using System.Collections;
using UnityEngine;

namespace Bullets
{
    public enum AttackType
    {
        Radial,
        Spiral,
        Flower,
        PianoWaterfall,
        Vinyl,
        Glissando
    }
    [CreateAssetMenu(menuName = "BulletHell System / Attack Pattern")]
    public class RadialShotPattern : ScriptableObject
    {
        [Header("Pattern Type")] public AttackType attackType;
        
        [Header("Settings")]
        public int Repetitions;
        [Range(-180f, 180f)] public float angleOffsetBetweenReps = 0f;
        public float startWait = 0f;
        public float endWait = 0f;
        public RadialShotSettings[] patternSettings;

        [Header("Extra Settings")] public int numberOfBullets;
        public int numberOfPetals;
        public float bulletSpeed;
        public float rotationSpeed;


        public IEnumerator ExecutePattern(Transform bossTransform, System.Action<bool> onComplete)
        {
            yield return new WaitForSeconds(startWait);
            
            Vector2 origin = bossTransform.position;
            Vector2 aimDirection = bossTransform.up;
            float currentSpiralAngle = 0f;

            for (int r = 0; r < Repetitions; r++)
            {
                //idk wtf is that
                RadialShotSettings currentSetting = (patternSettings != null && patternSettings.Length > 0) 
                    ? patternSettings[r % patternSettings.Length] 
                    : null;


                switch (attackType)
                {
                    case AttackType.Radial:
                        if (currentSetting != null)
                        {
                            ShotAttack.RadialShot(origin, aimDirection, currentSetting);
                        }

                        break;
                    case AttackType.Spiral:
                    case AttackType.Vinyl:
                        if (currentSetting != null)
                        {
                            Vector2 spiralDir = aimDirection.Rotate(currentSpiralAngle);
                            ShotAttack.RadialShot(origin, spiralDir, currentSetting);
                            currentSpiralAngle += rotationSpeed;
                        }
                        break;

                    case AttackType.Flower:
                        ShotAttack.FlowerShot(origin, aimDirection, numberOfBullets, numberOfPetals, bulletSpeed, currentSetting);
                        break;

                    case AttackType.PianoWaterfall:
                        ShotAttack.PianoWaterfall(origin, Time.time, bulletSpeed, currentSetting);
                        break;
                    
                }
                
                float waitTime = (currentSetting != null) ? currentSetting.cooldownTime : 0.2f;
                yield return new WaitForSeconds(waitTime);
            }
            
            yield return new WaitForSeconds(endWait);
            onComplete?.Invoke(true);
        }
    }
}
*/
