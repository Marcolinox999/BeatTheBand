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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //IMPORTANT TO DELETE THIS IT IS JUST FOR TESTING PURPOSES
        {
            ApplyDamage(1f);
            Debug.Log("Damage dealt" + base.CurrentHealth);
        }
    }
}
