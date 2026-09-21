using System.Collections;
using UnityEngine;

namespace Bullets
{
    public class RadialShotWeapon : MonoBehaviour
    {
        [SerializeField] private RadialShotPattern shotPattern;

        private bool onShotPattern = false;

        private void Update()
        {
            if (onShotPattern)
                return;

            StartCoroutine(ExecuteRadialShotPattern(shotPattern));
        }

        private void OnDisable()
        {
            onShotPattern = false;
        }

        private IEnumerator ExecuteRadialShotPattern(RadialShotPattern pattern)
        {
            onShotPattern = true;

            int lap = 0;
            Vector2 aimDirection = transform.up;
            Vector2 center = transform.position;

            yield return new WaitForSeconds(pattern.startWait);

            while (lap < pattern.Repetitions)
            {
                if (lap > 0 && pattern.angleOffsetBetweenReps != 0f)
                    aimDirection = aimDirection.Rotate(pattern.angleOffsetBetweenReps);

                for (int i = 0; i < pattern.patternSettings.Length; i++)
                {
                    ShotAttack.RadialShot(
                        center,
                        aimDirection,
                        pattern.patternSettings[i]
                    );

                    yield return new WaitForSeconds(
                        pattern.patternSettings[i].cooldownTime
                    );
                }

                lap++;
            }

            yield return new WaitForSeconds(pattern.endWait);

            onShotPattern = false;
        }

        public float GetPatternDuration()
        {
            float duration = shotPattern.startWait;

            float lapDuration = 0f;

            foreach (RadialShotSettings settings in shotPattern.patternSettings)
            {
                lapDuration += settings.cooldownTime;
            }

            duration += lapDuration * shotPattern.Repetitions;
            duration += shotPattern.endWait;

            return duration;
        }
    }
}