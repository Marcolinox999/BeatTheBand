using UnityEngine;

public class PlayerHealth : BaseHealth
{
    
    [Header("Particles")] [SerializeField] private ParticleSystem[] playerParticles;
    
    protected override void Start()
    {
        base.Start();
        Init();
    }

    protected override void ParticleDamage()
    {
        Debug.Log("¡Intentando reproducir partículas!");
        for (int i = 0; i < playerParticles.Length; i++)
        {
            if (playerParticles[i] != null)
            {
                playerParticles[i].Play();
            }
        }
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
