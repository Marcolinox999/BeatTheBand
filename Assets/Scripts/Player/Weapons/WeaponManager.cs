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
    public int weaponID = 0;
    private bool canSwitch = true;
    [Header("Trumpet")] [SerializeField] private int trumpetMiss;
    [Header("Drums")] [SerializeField] private int drumsMiss;
    [Header("Cymbals")]  [SerializeField] private int cymbalsMiss;
    private int accumulated;
    private bool canFire = false;
    [Header("Accordion")] [SerializeField] private int accordionmMiss;
    private int streak;

    private int missBeats = 1;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
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
                        missBeats = trumpetMiss; ;
                        _actualWeapon =  Weapons.Trumpet;
                        break;
                    case 1:
                        missBeats = drumsMiss;
                        _actualWeapon = Weapons.Drum;
                        break;
                    case 2:
                        missBeats = cymbalsMiss;
                        _actualWeapon = Weapons.Cymbals;
                        break;
                    case 3:
                        missBeats = accordionmMiss;
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
        switch (score)
        {
            case BeatManager.Score.Missed:
                
                return;

            case BeatManager.Score.Ok:
                Instantiate(bulletPrefabs[0], transform.position, Quaternion.identity);
                break;

            case BeatManager.Score.Perfect:
                Instantiate(bulletPrefabs[1], transform.position,Quaternion.identity);
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
                StartCoroutine(ShotCycles(0.1f,3,bulletPrefabs[2]));
                break;

            case BeatManager.Score.Perfect:
                StartCoroutine(ShotCycles(0.1f,4,bulletPrefabs[3]));
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
                int randomRotation = Random.Range(-15, 15);
                if (accumulated >= 6)
                    Instantiate(bulletPrefabs[5], transform.position, Quaternion.Euler(transform.rotation.x,transform.rotation.y,randomRotation + transform.rotation.z));
                else
                    Instantiate(bulletPrefabs[4], transform.position, Quaternion.Euler(transform.rotation.x,transform.rotation.y,randomRotation + transform.rotation.z));
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
                if (streak <= 14)
                {
                    streak++;
                    StartCoroutine(ShotCycles(0.1f, streak, bulletPrefabs[6]));
                }
                else
                {
                    StartCoroutine(ShotCycles(0.1f, streak, bulletPrefabs[7]));
                }

                break;

            case BeatManager.Score.Perfect:
                streak += 2;
                StartCoroutine(ShotCycles(0.1f,streak,bulletPrefabs[7]));
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

    private IEnumerator ShotCycles(float fireRate, int repetitions, GameObject bullet)
    {
        for (int i = 0; i < repetitions; i++)
        { 
            int randomRotation = Random.Range(-3, 3);
            Instantiate(bullet, transform.position, Quaternion.Euler(transform.rotation.x,transform.rotation.y,randomRotation + transform.rotation.z));
            yield return new WaitForSeconds(fireRate);
        }
    }

    
}
