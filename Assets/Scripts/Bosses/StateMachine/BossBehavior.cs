using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 1000f;

    [Header("Phases")]
    [SerializeField] private BossPhase[] phases;

    private float currentHealth;
    private int currentPhase;

    private BossStateMachine stateMachine;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public int CurrentPhase => currentPhase;

    public BossPhase CurrentPhaseData => phases[currentPhase - 1];

    private void Awake()
    {
        currentHealth = maxHealth;
        currentPhase = 1;

        stateMachine = new BossStateMachine();

        stateMachine.ChangeState(
            new BossPhaseState(this, currentPhase)
        );
    }

    private void Update()
    {
        stateMachine.Update();

        // TEMPORAL:
        // Se eliminará cuando conectemos el sistema de daño real.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(100f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0f)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f);

        CheckPhase();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void CheckPhase()
    {
        if (currentHealth <= 0f)
            return;

        if (currentPhase >= phases.Length)
            return;

        int nextPhase = currentPhase + 1;

        BossPhase nextPhaseData = phases[nextPhase - 1];

        if (currentHealth <= nextPhaseData.healthThreshold)
        {
            ChangePhase(nextPhase);
        }
    }

    private void ChangePhase(int newPhase)
    {
        Debug.Log(
            $"Boss comenzando transición a Fase {newPhase}"
        );

        ChangeState(
            new BossTransitionState(this, newPhase)
        );
    }

    public BossPhase GetPhaseData(int phase)
    {
        return phases[phase - 1];
    }

    public void SetCurrentPhase(int phase)
    {
        currentPhase = phase;

        Debug.Log(
            $"Boss ha entrado en Fase {currentPhase}"
        );
    }

    public void ChangeState(BossState newState)
    {
        stateMachine.ChangeState(newState);
    }

    private void Die()
    {
        Debug.Log("Boss derrotado");
    }
}