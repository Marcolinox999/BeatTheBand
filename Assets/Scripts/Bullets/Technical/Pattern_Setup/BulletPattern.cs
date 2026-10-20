using System.Collections;
using UnityEngine;

namespace Bullets
{
    public enum AttackType
    {
        Radial,
        Spiral,
        Flower,
        PianoWaterfall,
        ConeToPlayer,
        LineToPlayer
    }

    [CreateAssetMenu(
        menuName = "BulletHell System/Bullet Pattern",
        fileName = "BulletPattern"
    )]
    public class BulletPattern : ScriptableObject
    {
        [Header("Pattern Settings")]
        public int repetitions = 1;
        [Range(-180f, 180f)]
        public float angleOffsetBetweenReps = 0f;
        
        public float delayBetweenReps = 0f;
        
        public float startWait = 0f;
        public float endWait = 0f;

        [Header("Steps")]
        public BulletPatternStep[] steps;

        public IEnumerator Execute(Transform origin, Transform player)
        {
            if (steps == null || steps.Length == 0) yield break;
            
            if (startWait > 0f)
            {
                yield return new WaitForSeconds(startWait);
            }

            Vector2 aimDirection = origin.up;
            
            for (int repetition = 0; repetition < repetitions; repetition++)
            {
                if (repetition > 0 &&
                    angleOffsetBetweenReps != 0f)
                {
                    aimDirection = aimDirection.Rotate(angleOffsetBetweenReps);
                }
                
                    
                    foreach (BulletPatternStep step in steps)
                    {
                        if (step == null)
                            continue;

                        ExecuteStep(
                            step,
                            origin,
                            player,
                            aimDirection
                        );

                        if (step.delayAfterStep > 0f)
                        {
                            yield return new WaitForSeconds(step.delayAfterStep);
                        }
                    }
                    
                    if (delayBetweenReps > 0f && repetition < repetitions - 1)
                    {
                        yield return new WaitForSeconds(delayBetweenReps);
                    }
                
            }
            
            if (endWait > 0f)
            {
                yield return new WaitForSeconds(endWait);
            }
        }

        private void ExecuteStep(
            BulletPatternStep step,
            Transform origin,
            Transform player,
            Vector2 aimDirection)
        {
            Vector2 position = origin.position;

            switch (step.attackType)
            {
                case AttackType.Radial:

                    ShotAttack.RadialShot(position, aimDirection, step.settings);
                    break;


                case AttackType.Spiral:

                    Vector2 spiralDirection = aimDirection.Rotate(step.rotation);

                    ShotAttack.RadialShot(position, spiralDirection, step.settings);
                    break;


                case AttackType.Flower:

                    ShotAttack.FlowerShot(position, aimDirection, step.settings.numberOfBullets, step.numberOfPetals, step.settings.bulletSpeed, step.settings);
                    break;


                case AttackType.PianoWaterfall:

                    ShotAttack.PianoWaterfall(position, step.settings.numberOfBullets, step.waterfallWidth, step.waterfallEdge, step.settings.bulletSpeed, step.settings);
                    break;


                case AttackType.ConeToPlayer:

                    if (player == null)
                    {
                        Debug.LogWarning("BulletPattern: ConeToPlayer necesita un Player.");
                        return;
                    }

                    ShotAttack.ConeShotToPlayer(position, player.position, step.settings, step.settings.numberOfBullets, step.coneAngle, step.settings.bulletSpeed);
                    break;


                case AttackType.LineToPlayer:

                    if (player == null)
                    {
                        Debug.LogWarning("BulletPattern: LineToPlayer necesita un Player.");
                        return;
                    }

                    ShotAttack.LineShotToPlayer(position, player.position, step.settings.numberOfBullets, step.spacing, step.settings.bulletSpeed, step.settings);
                    break;
            }
        }
    }


    [System.Serializable]
    public class BulletPatternStep
    {
     
        public AttackType attackType;
        
        [Header("Cooldown")]
        public float delayAfterStep = 0.5f;
        
        public RadialShotSettings settings;
        
        
        [Header("PianoWaterfall")]
        public ShotAttack.WaterfallEdge waterfallEdge;

        public float waterfallWidth = 10f;

        [Header("Spiral")]
        public float rotation = 10f;

        [Header("Flower")]
        public int numberOfPetals = 5; 

        [Header("Cone")]
        [Range(0f, 360f)]
        public float coneAngle = 60f;
        
        [Header("Line")]
        public float spacing = 1f;
    }
}