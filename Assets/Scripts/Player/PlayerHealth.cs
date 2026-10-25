using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : BaseHealth
{
    
    [Header("Particles")] [SerializeField] private ParticleSystem[] playerParticles;
    [SerializeField] private string mapName = "Game Over";
    [SerializeField] private BasicBoss boss;
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
        if (boss != null)
            GameManager.Instance.bossLeftHealth = boss.CurrentHealth;
        GameManager.Instance.LoadScene(mapName);
    }

    private void Update()
    {
        
    }
}
