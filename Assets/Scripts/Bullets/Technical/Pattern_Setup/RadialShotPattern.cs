using UnityEngine;

namespace Bullets
{
    [CreateAssetMenu(menuName = "BulletHell System / Radial Shot Pattern")]
    public class RadialShotPattern : ScriptableObject
    {
        public int Repetitions;
        [Range(-180f, 180f)] public float angleOffsetBetweenReps = 0f;
        public float startWait = 0f;
        public float endWait = 0f;
        public RadialShotSettings[] patternSettings;
    }
}
