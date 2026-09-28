using UnityEngine;

public class BossBehavior : BaseHealth
{
    [Header("Phases")]
    [SerializeField] private BossPhase[] phases;
    
    private int currentPhase;

    private BossStateMachine stateMachine;
    
    public int CurrentPhase => currentPhase;

    public BossPhase CurrentPhaseData => phases[currentPhase - 1];

    private void Awake()
    {
        currentPhase = 1;

        stateMachine = new BossStateMachine();

        stateMachine.ChangeState(new BossPhaseState(this, currentPhase));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //IMPORTANT TO DELETE THIS IT IS JUST FOR TESTING PURPOSES
        {
            ApplyDamage(100f);
            Debug.Log("Damage dealt" + base.CurrentHealth);
        }
    }

    private void CheckPhase()
    {
        if (base.CurrentHealth <= 0f)
            return;

        if (currentPhase >= phases.Length)
            return;

        int nextPhase = currentPhase + 1;

        BossPhase nextPhaseData = phases[nextPhase - 1];

        if (base.CurrentHealth <= nextPhaseData.healthThreshold)
        {
            ChangePhase(nextPhase);
        }
    }

    private void ChangePhase(int newPhase)
    {
        ChangeState(new BossTransitionState(this, newPhase));
    }

    public BossPhase GetPhaseData(int phase)
    {
        return phases[phase - 1];
    }

    public void SetCurrentPhase(int phase)
    {
        currentPhase = phase;
    }

    public void ChangeState(BossState newState)
    {
        stateMachine.ChangeState(newState);
    }

    protected override void Die()
    {
        Debug.Log("Boss dies of death");
    }
}