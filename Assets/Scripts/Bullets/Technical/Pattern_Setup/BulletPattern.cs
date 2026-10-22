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
        LineToPlayer,
        Boomerang,
        StarBurst,
        StopAndGo,
        Cage,
        SpinningRadial
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
                        
                        int reps = Mathf.Max(1, step.stepRepetitions);
                        for (int i = 0; i < reps; i++)
                        {
                            ExecuteStep(
                                step,
                                origin,
                                player,
                                aimDirection,
                                i
                            );

                            if (step.delayAfterStep > 0f)
                            {
                                yield return new WaitForSeconds(step.delayAfterStep);
                            }
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
            Vector2 aimDirection,
            int i)
        {
            Vector2 position = origin.position;

            switch (step.attackType)
            {
                case AttackType.Radial:

                    ShotAttack.RadialShot(position, aimDirection, step.settings);
                    break;


                case AttackType.Spiral:
                    float currentRotation = step.rotation * i;
                    
                    Vector2 spiralDirection = aimDirection.Rotate(currentRotation);

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
                
                case AttackType.Boomerang:
                    ShotAttack.BoomerangRadial(position, aimDirection, step.returnForce, step.settings);
                    break;

                case AttackType.StarBurst:
                    ShotAttack.StarBurst(position, aimDirection, step.starPoints, step.bulletsPerEdge, step.settings.bulletSpeed, step.settings);
                    break;

                case AttackType.StopAndGo:
                    ShotAttack.StopAndGoRadial(position, aimDirection, step.timeToStop, step.timeToGo, step.goSpeed, step.settings);
                    break;

                case AttackType.Cage:
                    if (player == null) return;
                    ShotAttack.CageShot(player.position, step.cageRadius, step.gapAngle, step.settings);
                    break;
                
                case AttackType.SpinningRadial:
                    ShotAttack.SpinningRadial(position, aimDirection, step.spinSpeed, step.settings);
                    break;
            }
        }
    }


    [System.Serializable]
    public class BulletPatternStep
    {
     
        public AttackType attackType;

        [Header("Repetitions")] [Tooltip("How much Reps the attack takes")]
        public int stepRepetitions = 1;
        [Header("Cooldown")]
        [Tooltip("Cooldown from every Shot")]
        public float delayAfterStep = 0.5f;
        
        public RadialShotSettings settings;
        
        [Header("Spinning Radial")]
        [Tooltip("Positivo gira anti-horario, Negativo gira horario")]
        
        public float spinSpeed = 45f;
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
        
        [Header("Boomerang")]
        public float returnForce = 15f;

        [Header("StarBurst")]
        public int starPoints = 5;      
        public int bulletsPerEdge = 5;   

        [Header("Stop And Go")]
        public float timeToStop = 1f;    
        public float timeToGo = 1.5f;   
        public float goSpeed = 10f;    

        [Header("Cage")]
        [Range(0f, 360f)]
        public float gapAngle = 45f;    
        public float cageRadius = 15f;
    }
}