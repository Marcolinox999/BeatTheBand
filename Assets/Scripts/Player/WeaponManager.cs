using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    private enum Weapons
    {
        Trumpet,
        Drum,
        Cymbals,
        Accordion
    }

    [SerializeField] private InputActionReference weaponSwitch;
    [SerializeField] private float switchTime = 0.5f;
    private Weapons _actualWeapon =  Weapons.Trumpet;
    private int weaponID = 0;
    private bool canSwitch = true;

    private void OnEnable()
    {
        weaponSwitch.action.Enable();
    }

    private void OnDisable()
    {
        weaponSwitch.action.Disable();
    }
    private void Update()
    {
        if (weaponSwitch.action.WasPressedThisFrame() && canSwitch)
        {
            StartCoroutine(TimeBetweenSwitch());
            WeaponSwitch();
        }
        
    }

    private void WeaponSwitch()
    {
        for (int i = weaponID +1; i <= GameManager.Instance.unlockedWeapons.Length; i++)
        {
            Debug.Log(weaponID);
            if (i >= GameManager.Instance.unlockedWeapons.Length)
            {
                i = 0;
            }
            if (GameManager.Instance.unlockedWeapons[i])
            {
                weaponID = i;
                switch (weaponID)
                {
                    case 0:
                        _actualWeapon =  Weapons.Trumpet;
                        break;
                    case 1:
                        _actualWeapon = Weapons.Drum;
                        break;
                    case 2:
                        _actualWeapon = Weapons.Cymbals;
                        break;
                    case 3:
                        _actualWeapon = Weapons.Accordion;
                        break;
                    
                }

                return;
            }
        }
    }

    private void Trumpet()
    {
        
    }

    private void Drum()
    {
        
    }

    private void Cymbals()
    {
        
    }

    private void Accordion()
    {
        
    }

    private IEnumerator TimeBetweenSwitch()
    {
        canSwitch = false;
        yield return new WaitForSeconds(switchTime);
        canSwitch = true;
    }

    private void WeaponAttack()
    {
        switch (_actualWeapon)
        {
            case Weapons.Trumpet:
                Trumpet();
                break;
            case Weapons.Drum:
                Drum();
                break;
            case Weapons.Cymbals:
                Cymbals();
                break;
            case Weapons.Accordion:
                Accordion();
                break;
        }
    }
}
