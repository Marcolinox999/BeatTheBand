using System;
using System.Collections;
using UnityEngine;

public class RedemptionSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeToHeal = 2f;
    [SerializeField] private float healAmount = 1f;
    [SerializeField] private float healingRadius = 2f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Visuals")] [SerializeField] private Transform growingIndicator;
    [SerializeField] private Transform fixedIndicator;
    [SerializeField] private Vector3 maxScale = new Vector3(3f, 3f, 1f);

    private void Start()
    {
        if (fixedIndicator != null) fixedIndicator.localScale = maxScale;
        if (growingIndicator != null) growingIndicator.localScale = Vector3.zero;
        StartCoroutine(RedemptionRoutine());
    }

    private IEnumerator RedemptionRoutine()
    {
        float elapsedTime = 0f;
        Vector3 initialScale = Vector3.zero;
        
        while (elapsedTime < timeToHeal)
        {
            elapsedTime += Time.deltaTime;
            if (growingIndicator != null)
            {
                growingIndicator.localScale = Vector3.Lerp(initialScale, maxScale, elapsedTime / timeToHeal);
            }
            yield return null;
        }
        
        TriggerHeal();
        
        Destroy(gameObject);
    }

    private void TriggerHeal()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, healingRadius, playerLayer);
        foreach (Collider2D col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                BaseHealth playerHealth = col.GetComponent<BaseHealth>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(healAmount);
                }
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.lightGreen;
        Gizmos.DrawWireSphere(transform.position, healingRadius);
    }
}
