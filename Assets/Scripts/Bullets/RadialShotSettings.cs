using UnityEngine;

namespace Bullets
{
    [System.Serializable]
    public class RadialShotSettings
    {
        [Header("Base Settings")]
        public int numberOfBullets;
        public float bulletSpeed;
        public float cooldownTime;
    
        [Header("Offsets")]
        [Range(-100f, 100f)] public float angleOffset = 0f;
        [Range(-1f,1f)] public float phaseOffset = 0f;
    
        [Header ("Mask")]
        public bool radialMask;
        [Range(0f, 360f)] public float maskAngle = 360f; 
    }
}
