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

                    ShotAttack.FlowerShot(position, aimDirection, step.numberOfBullets, step.numberOfPetals, step.bulletSpeed, step.settings);
                    break;


                case AttackType.PianoWaterfall:

                    ShotAttack.PianoWaterfall(position, step.bulletSpeed, step.settings);
                    break;


                case AttackType.ConeToPlayer:

                    if (player == null)
                    {
                        Debug.LogWarning("BulletPattern: ConeToPlayer necesita un Player.");
                        return;
                    }

                    ShotAttack.ConeShotToPlayer(position, player.position, step.settings, step.numberOfBullets, step.coneAngle, step.bulletSpeed);
                    break;


                case AttackType.LineToPlayer:

                    if (player == null)
                    {
                        Debug.LogWarning("BulletPattern: LineToPlayer necesita un Player.");
                        return;
                    }

                    ShotAttack.LineShotToPlayer(position, player.position, step.numberOfBullets, step.spacing, step.bulletSpeed, step.settings);
                    break;
            }
        }
    }


    [System.Serializable]
    public class BulletPatternStep
    {
        public AttackType attackType;
        public RadialShotSettings settings;


        [Header("Spiral")]
        public float rotation = 10f;

        [Header("Flower")]
        public int numberOfBullets = 12;
        public int numberOfPetals = 5; 
        public float bulletSpeed = 5f;

        [Header("Cone")]
        [Range(0f, 360f)]
        public float coneAngle = 60f;
        
        [Header("Line")]
        public float spacing = 1f;
    }
}