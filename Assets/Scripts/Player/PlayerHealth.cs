using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    protected override void Start()
    {
        base.Start();
        Init();
    }

    protected override void SpriteDamage()
    {
        CallDamageFlash();
    }
    
    protected override void Die()
    {
        Debug.Log("Player has died");
    }
}
