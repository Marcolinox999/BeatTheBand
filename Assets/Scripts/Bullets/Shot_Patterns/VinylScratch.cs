using System;
using System.Collections;
using UnityEngine;

namespace Bullets
{
    public class VinylScratch : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private AudioClip scratchAudioClip;    
        [SerializeField] private float scratchAngle = -90f; 
        [SerializeField] private float scratchDuration = 0.2f;

        [Header("Spiral Settings")] 
        [SerializeField] private float rotationSpeedPerShot;
        private float currentSpiralAngle = 0f;
        private int spiralDirectionMultiplier = 1;
        
        [Header("Refs")]
        [SerializeField] private RadialShotSettings settings;
        private AudioSource audioSource;
        
        private bool isScratching = false;
        private Vector2 currentAimDirection;


        private void Start()
        {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
            
                currentAimDirection = transform.up;
                if (settings != null && settings.cooldownTime > 0f)
                {
                    StartCoroutine(ConstantShootingRoutine());
                }
                else
                {
                    Debug.LogWarning("RadialShotSettings no está asignado o su cooldown es 0 en VinylScratch.");
                }
        }
        
        public void TriggerVinylScratch()
        {
            if (!isScratching)
            {
                StartCoroutine(ExecuteVinylScratchRoutine());
            }
        }
        
        private IEnumerator ConstantShootingRoutine()
        {
            while (true)
            {
                if (!isScratching && settings != null)
                {
                    Vector2 baseDir = transform.up;
                    Vector2 spiralDir = baseDir.Rotate(currentSpiralAngle);
                    ShotAttack.RadialShot(transform.position, spiralDir, settings);

                    //currentSpiralAngle += rotationSpeedPerShot;
                    //SI QUEREIS QUE SE PARE CUANDO HACE X DAÑO Y LEUGO SIGA +45
                    
                    currentSpiralAngle += rotationSpeedPerShot * spiralDirectionMultiplier;
                }
                yield return new WaitForSeconds(settings.cooldownTime);
            }
        }

        private IEnumerator ExecuteVinylScratchRoutine()
        {
            isScratching = true;
            
            if(scratchAudioClip != null) audioSource.PlayOneShot(scratchAudioClip);
            
            spiralDirectionMultiplier *= -1;
            
            Quaternion startRot = transform.rotation;
            Quaternion targetRot = startRot * Quaternion.Euler(0, 0, scratchAngle);
            
            float elapsed = 0f;
            while (elapsed < scratchDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / scratchDuration;
                transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
                yield return null;
            }
            
            currentSpiralAngle += 45f;
            yield return new WaitForSeconds(0.3f);
            isScratching = false;
        }
        
    }
    
    

}
