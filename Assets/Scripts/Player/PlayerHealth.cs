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
        
    }
}
