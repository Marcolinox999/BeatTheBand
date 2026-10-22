using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasicBoss : BaseHealth
{
    public enum Phases
    {
        phase1,
        phase2,
        phase3,
        dead
    }
    private BasicMovementBoss basicBoss;
    [Header("Boss")]
    [SerializeField] private BossPhase[] phasePack;

    private Phases currentPhase = Phases.phase1;
    private int currentPack;
    private bool isDead;
    private Coroutine attackCoroutine;

    private void OnEnable()
    {
        currentPhase = Phases.phase1;
        currentPack = 0;
        isDead = false;
        
        basicBoss = GetComponent<BasicMovementBoss>();
        basicBoss.bossSpeed = phasePack[currentPack].bossSpeed;
        DisableAllAttacks();

        if (phasePack == null || phasePack.Length == 0)
        {
            return;
        }

        PhaseSwitch();
    }

    private void Update()
    {
        if (isDead)
            return;

        // Solo para probar el jefe.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyDamage(10);
        }

        CheckPhaseTransition();
    }

    private void CheckPhaseTransition()
    {
        if (phasePack == null || currentPack >= phasePack.Length)
            return;
        if (CurrentHealth > phasePack[currentPack].healthThreshold)
            return;
        if (currentPack + 1 < phasePack.Length)
        {
            currentPack++;
            currentPhase = (Phases)currentPack;
            basicBoss.bossSpeed = phasePack[currentPack].bossSpeed;
            PhaseSwitch();
        }
        else
        {
            currentPhase = Phases.dead;
            PhaseSwitch();
        }
    }

    private void PhaseSwitch()
    {
        basicBoss.MovemeventSwitch(phasePack[currentPack].movingType, phasePack[currentPack].movingStyle);
        switch (currentPhase)
        {
            case Phases.phase1:
            case Phases.phase2:
            case Phases.phase3:
                StartAttackLoop();
                break;

            case Phases.dead:
                if (attackCoroutine != null)
                    StopCoroutine(attackCoroutine);
                Die();
                break;
        }
    }
    private IEnumerator AttackLoop()
    {
        while (!isDead)
        {
            var attacks = phasePack[currentPack].attacks;

            if (attacks == null || attacks.Length == 0)
                yield break;

            int randomIndex = Random.Range(0, attacks.Length);
            GameObject selectedAttack = attacks[randomIndex].gameObject;

            selectedAttack.SetActive(true);

            yield return new WaitForSeconds(phasePack[currentPack].attackDuration);

            selectedAttack.SetActive(false);

            yield return new WaitForSeconds(phasePack[currentPack].waitBetweenAttacks);
        }
    }
    private void StartAttackLoop()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        DisableAllAttacks();

        attackCoroutine = StartCoroutine(AttackLoop());
    }

    private void DisableAllAttacks()
    {
        if (phasePack == null)
            return;

        foreach (BossPhase phase in phasePack)
        {
            if (phase.attacks == null)
                continue;

            foreach (var attack in phase.attacks)
            {
                if (attack != null)
                    attack.gameObject.SetActive(false);
            }
        }
    }
    protected override void SpriteDamage()
    {
        if (isDead) return;
        CurrentHealth -= 10;
        CheckPhaseTransition();
    }
    protected override void ParticleDamage()
    {
    }
    protected override void Die()
    {
        if (isDead) return;
        isDead = true;
        DisableAllAttacks();
        basicBoss.bossSpeed = 0;
        Debug.Log("Boss Dead");
    }
}