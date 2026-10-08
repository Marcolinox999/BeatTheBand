using Bullets;
using UnityEngine;

public class BossBehavior : BaseHealth
{
    [Header("Phases")]
    [SerializeField] private BossPhase[] phases;
    
    [Header("Vinyl Scratch Mechanic")]
    [SerializeField] private float scratchDamageThreshold = 50f;
    private float accumulatedDamage = 0f;
    [SerializeField] VinylScratch vinylScratchComponent;
    
    private int currentPhase;
    private BossStateMachine stateMachine;
    
    public int CurrentPhase => currentPhase;

    public BossPhase CurrentPhaseData => phases[currentPhase - 1];

    private void Awake()
    {
        currentPhase = 1;

        stateMachine = new BossStateMachine();

        //stateMachine.ChangeState(new BossPhaseState(this, currentPhase));
    }

    protected override void Start()
    {
        base.Start();
        Init();
        
        if (vinylScratchComponent == null)
        {
            Debug.LogError("¡Falta el componente VinylScratch en este GameObject!");
        }
        stateMachine.ChangeState(new BossPhaseState(this, currentPhase));
    }

    protected override void SpriteDamage()
    {
        CallDamageFlash();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //IMPORTANT TO DELETE THIS IT IS JUST FOR TESTING PURPOSES
        {
            ApplyDamage(20);
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
    
    public override void ApplyDamage(float damage)
    {
        base.ApplyDamage(damage);
        if (damage <= 0f || CurrentHealth <= 0f) return;
            
        accumulatedDamage += damage;

        if (accumulatedDamage >= scratchDamageThreshold)
        {
            accumulatedDamage = 0f;
            if (vinylScratchComponent != null)
            {
                vinylScratchComponent.TriggerVinylScratch();
            }
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