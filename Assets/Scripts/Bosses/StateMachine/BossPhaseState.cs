using UnityEngine;

public class BossPhaseState : BossState
{
    private int phase;

    private GameObject[] attacks;

    private int lastAttackIndex = -1;

    public BossPhaseState(
        BossBehavior boss,
        int phase) : base(boss)
    {
        this.phase = phase;
    }

    public override void Enter()
    {
        Debug.Log($"Entrando en Fase {phase}");

        attacks = boss.CurrentPhaseData.attacks;

        SelectAttack();
    }

    private void SelectAttack()
    {
        if (attacks == null || attacks.Length == 0)
        {
            Debug.LogWarning(
                $"La Fase {phase} no tiene ataques asignados."
            );

            return;
        }

        int attackIndex = GetRandomAttackIndex();

        GameObject attack = attacks[attackIndex];

        if (attack == null)
        {
            Debug.LogWarning(
                $"El ataque {attackIndex} de la Fase {phase} es null."
            );

            return;
        }

        lastAttackIndex = attackIndex;

        boss.ChangeState(
            new BossAttackState(boss, attack)
        );
    }

    private int GetRandomAttackIndex()
    {
        if (attacks.Length <= 1)
            return 0;

        int index;

        do
        {
            index = Random.Range(0, attacks.Length);
        }
        while (index == lastAttackIndex);

        return index;
    }

    public override void Exit()
    {
        Debug.Log($"Saliendo de Fase {phase}");
    }
}