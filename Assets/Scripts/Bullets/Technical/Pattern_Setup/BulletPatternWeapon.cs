using System;
using System.Collections;
using UnityEngine;

namespace Bullets
{
    public class BulletPatternWeapon : MonoBehaviour
    {
        [SerializeField] private BulletPattern pattern;
        [SerializeField] private Transform player;

        private Coroutine patternCoroutine;

        public bool IsRunning => patternCoroutine != null;

        public event Action PatternFinished;

        public void Play()
        {
            Stop();

            if (pattern == null)
            {
                Debug.LogWarning(
                    $"{name}: No hay BulletPattern asignado."
                );
                return;
            }

            patternCoroutine = StartCoroutine(RunPattern());
        }

        public void Stop()
        {
            if (patternCoroutine == null)
                return;

            StopCoroutine(patternCoroutine);
            patternCoroutine = null;
        }

        private IEnumerator RunPattern()
        {
            yield return pattern.Execute(transform, player);

            patternCoroutine = null;
            PatternFinished?.Invoke();
        }

        private void OnDisable()
        {
            Stop();
        }
    }
}