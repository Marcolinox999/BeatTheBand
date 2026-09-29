using System;
using System.Collections;
using System.Collections.Generic;
using Bullets;
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

    private void Trumpet(BeatManager.Score score)
    {
        //esto es hasta que hagamos lo de apuntar
        Vector2 direction = transform.up;
        switch (score)
        {
            case BeatManager.Score.Missed:
                return;

            case BeatManager.Score.Ok:
                ShotAttack.TrumpetShot(transform.position, direction, 8f);
                break;

            case BeatManager.Score.Perfect:
                ShotAttack.TrumpetShot(transform.position, direction.Rotate(5f), 8f);
                ShotAttack.TrumpetShot(transform.position, direction, 8f);
                ShotAttack.TrumpetShot(transform.position, direction.Rotate(-5f), 8f);
                
                break;
        }
    }

    private void Drum(BeatManager.Score score)
    {
    }

    private void Cymbals(BeatManager.Score score)
    {
    }

    private void Accordion(BeatManager.Score score)
    {
    }

    private IEnumerator TimeBetweenSwitch()
    {
        canSwitch = false;
        yield return new WaitForSeconds(switchTime);
        canSwitch = true;
    }
    
    public void Attack()
    {
        BeatManager.Score score = BeatManager.instance.PrecisionCheck();
        switch (_actualWeapon)
        {
            case Weapons.Trumpet:
                Trumpet(score);
                break;
            case Weapons.Drum:
                Drum(score);
                break;
            case Weapons.Cymbals:
                Cymbals(score);
                break;
            case Weapons.Accordion:
                Accordion(score);
                break;
        }
    }
}
