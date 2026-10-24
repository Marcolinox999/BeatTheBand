using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealingSpawner : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject redemptionPrefab;
    
    [Header("Spawn Time")]
    [SerializeField] private float minSpawnTime = 5f;
    [SerializeField] private float maxSpawnTime = 12f;
    
    [Header("Map Limits")]
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
            
            float randomX = Random.Range(minBounds.x, maxBounds.x);
            float randomY = Random.Range(minBounds.y, maxBounds.y);
            Vector2 spawnPosition = new Vector2(randomX, randomY);
            
            Instantiate(redemptionPrefab, spawnPosition, Quaternion.identity);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector2 center = (minBounds + maxBounds) / 2f;
        Vector2 size = new Vector2(maxBounds.x - minBounds.x, maxBounds.y - minBounds.y);
        Gizmos.DrawWireCube(center, size);
    }
}
