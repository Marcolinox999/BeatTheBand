using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class MusicalReward : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 15;
    
    private Rigidbody rb;
    private AudioSource audioSource;
    [SerializeField] private AudioClip choque;
    [SerializeField] private AudioClip getIt;
    
    private SpriteRenderer spriteRenderer;

    private BossBehavior boss;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        KilledBoss();
    }

    public void KilledBoss()
    {
        transform.position = boss.transform.position;
        rb.linearVelocity = SetInicialDirection() * initialSpeed;
    }

    private Vector3 SetInicialDirection()
    {
        Vector3 direction;
        var rand = Random.Range(0, 2);
        switch (rand)
        {
            case 1:
                direction = new Vector3(1, 0.5f);
                break;
            case 2:
                direction = new Vector3(-1, 0.5f);
                break;
            case 3:
                direction = new Vector3(1, -0.5f);
                break;
            case 4:
                direction = new Vector3(-1, -0.5f);
                break;
            default: direction = Vector3.zero;
                break;
        }

        return direction;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Borders"))
        {
            AudioManager.instance.PlaySFX(choque);
        }
        else if  (other.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX(getIt);
            Destroy(gameObject);
        }
        
    }
    
}
