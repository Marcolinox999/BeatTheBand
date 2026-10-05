using System;
using NUnit.Framework;
using UnityEngine;

public class StatVisual : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private BaseHealth playerHealth;
    
    [Header("Heart Parts")]
    [SerializeField] SpriteRenderer heartMouth, heartHP, heartColor;
    
    [Header("Sprites")]
    [SerializeField] Sprite[] hpSprites, colorSprites, mouthSprites;

   /* private void Update()
    {
        int health = Mathf.RoundToInt(playerHealth.CurrentHealth);
        int index = Mathf.Clamp(health - 1, 0, hpSprites.Length - 1);
        heartColor = colorSprites[];
        
    }*/
}
