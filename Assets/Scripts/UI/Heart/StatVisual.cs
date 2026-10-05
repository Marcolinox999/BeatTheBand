using System;
using NUnit.Framework;
using UnityEngine;

public class StatVisual : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private BaseHealth playerHealth;

    [SerializeField] private WeaponManager currentWeapon;
    
    [Header("Heart Parts")]
    [SerializeField] SpriteRenderer heartMouth, heartHP, heartColor;
    
    [Header("Sprites")]
    [SerializeField] Sprite[] hpSprites, colorSprites, mouthSprites;

    private void Start()
    {
        heartColor.sprite = colorSprites[0];
    }

    private void Update()
    {
        int health = Mathf.RoundToInt(playerHealth.CurrentHealth);
        int index = Mathf.Clamp(health - 1, 0, hpSprites.Length - 1);
        heartHP.sprite = hpSprites[index];
        heartColor.sprite = colorSprites[currentWeapon.weaponID -1];
    }
}
