using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponManager : MonoBehaviour
{
    public enum Weapons
    {
        Trumpet,
        Drum,
        Cymbals,
        Accordion
    }

    [SerializeField] private InputActionReference weaponSwitch;
    [SerializeField] private float switchTime = 0.5f;
    
    [Header("References")]
    [SerializeField] private GameObject[] bulletPrefabs;
    //0:Trumpet 1:PTrumpet 2:Drum 3:PDrum 4:Cymbal 5:PCymbal 6:Accordion 7:PAccordion
    public Weapons _actualWeapon =  Weapons.Trumpet;
    private Transform hand;
    private ParticleSystem _particleSystem;
    public int weaponID = 0;
    private bool canSwitch = true;
    [SerializeField] private AudioClip switchSound;
    [Header("Trumpet")] [SerializeField] private int trumpetMiss;
    [SerializeField] private int trumpetSpread;
    [Header("Drums")] [SerializeField] private int drumsMiss;
    [SerializeField] private int drumsSpread;
    [Header("Cymbals")]  [SerializeField] private int cymbalsMiss;
    [SerializeField] private int cymbalsSpread;
    private int accumulated;
    private bool canFire = false;
    [Header("Accordion")] [SerializeField] private int accordionmMiss;
    [SerializeField] private int accordionSpread;
    private int streak;
    [Header("CrossHairs")]
    [SerializeField] private Sprite[] crossHair;
    private SpriteRenderer _crossHairRenderer;

    private int missBeats = 1;

    private void Awake()
    {
        hand = GetComponentInChildren<Transform>();
        _crossHairRenderer = GetComponentInChildren<SpriteRenderer>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

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
                        missBeats = trumpetMiss;
                        _crossHairRenderer.sprite = crossHair[0];
                        _actualWeapon =  Weapons.Trumpet;
                        break;
                    case 1:
                        missBeats = drumsMiss;
                        _crossHairRenderer.sprite = crossHair[1];
                        _actualWeapon = Weapons.Drum;
                        break;
                    case 2:
                        missBeats = cymbalsMiss;
                        _crossHairRenderer.sprite = crossHair[2];
                        _actualWeapon = Weapons.Cymbals;
                        break;
                    case 3:
                        missBeats = accordionmMiss;
                        _crossHairRenderer.sprite = crossHair[3];
                        _actualWeapon = Weapons.Accordion;
                        break;
                    
                }
                AudioManager.instance.PlaySFX(switchSound);
                return;
            }
        }
    }

    private void Trumpet(BeatManager.Score score)
    {
        //esto es hasta que hagamos lo de apuntar
        switch (score)
        {
            case BeatManager.Score.Missed:
                
                return;

            case BeatManager.Score.Ok:
                float randomRotation = Random.Range(-trumpetSpread, trumpetSpread);

                Quaternion rotation = hand.rotation * Quaternion.Euler(0f, 0f, randomRotation);
                Instantiate(bulletPrefabs[0], transform.position, rotation);
                break;

            case BeatManager.Score.Perfect:
                float randomRotationP = Random.Range(-trumpetSpread, trumpetSpread);

                Quaternion rotationP = hand.rotation * Quaternion.Euler(0f, 0f, randomRotationP);
                Instantiate(bulletPrefabs[1], transform.position, rotationP);
                break;
        }
    }

    private void Drum(BeatManager.Score score)
    {
        switch (score)
        {
            case BeatManager.Score.Missed:
                return;

            case BeatManager.Score.Ok:
                StartCoroutine(ShotCycles(0.1f,3,bulletPrefabs[2],drumsSpread));
                break;

            case BeatManager.Score.Perfect:
                StartCoroutine(ShotCycles(0.1f,4,bulletPrefabs[3],drumsSpread));
                break;
        }
    }

    private void Cymbals(BeatManager.Score score)
    {
        switch (score)
        {
            case BeatManager.Score.Missed:
                canFire  = true;
                break;

            case BeatManager.Score.Ok:
                accumulated += 2;
                break;

            case BeatManager.Score.Perfect:
                accumulated = 6;
                break;
        }

        if (canFire || accumulated >= 6)
        {
            for (int i = accumulated; i > 0; i--)
            {
                float randomRotation = Random.Range(-cymbalsSpread, cymbalsSpread);

                Quaternion rotation = hand.rotation * Quaternion.Euler(0f, 0f, randomRotation);
                if (accumulated >= 6)
                    Instantiate(bulletPrefabs[5], transform.position, rotation);
                else
                    Instantiate(bulletPrefabs[4], transform.position, rotation);
            }
            accumulated = 0;
            canFire = false;
        }
    }

    private void Accordion(BeatManager.Score score)
    {
        switch (score)
        {
            case BeatManager.Score.Missed:
                streak = 0;
                return;

            case BeatManager.Score.Ok:
                if (streak <= 8)
                {
                    streak++;
                    StartCoroutine(ShotCycles(0.05f, streak, bulletPrefabs[6],accordionSpread));
                }
                else
                {
                    StartCoroutine(ShotCycles(0.05f, streak, bulletPrefabs[7],accordionSpread));
                }

                break;

            case BeatManager.Score.Perfect:
                if (streak <= 10)
                    streak += 2;
                StartCoroutine(ShotCycles(0.05f,streak,bulletPrefabs[7],accordionSpread));
                break;
        }
        
    }

    private IEnumerator TimeBetweenSwitch()
    {
        canSwitch = false;
        yield return new WaitForSeconds(switchTime);
        canSwitch = true;
    }
    
    public event System.Action<BeatManager.Score> OnAttackScored;
    
    public void Attack()
    {
        BeatManager.Score score = BeatManager.instance.PrecisionCheck(missBeats);
        OnAttackScored?.Invoke(score);
        _particleSystem.Play();
        Debug.Log(missBeats);
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

    private IEnumerator ShotCycles(float fireRate, int repetitions, GameObject bullet, float spread)
    {
        for (int i = 0; i < repetitions; i++)
        { 
            float randomRotation = Random.Range(-spread, spread);

            Quaternion rotation = hand.rotation * Quaternion.Euler(0f, 0f, randomRotation);
            Instantiate(bullet, transform.position, rotation);
            yield return new WaitForSeconds(fireRate);
        }
    }

    
}
