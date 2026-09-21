using System.Collections;
using UnityEngine;
using Bullets;

public class BossAttackState : BossState
{
    private GameObject attack;
    private RadialShotWeapon weapon;

    private Coroutine attackCoroutine;

    public BossAttackState(
        BossBehavior boss,
        GameObject attack) : base(boss)
    {
        this.attack = attack;
    }

    public override void Enter()
    {
        if (attack == null)
        {
            Debug.LogWarning(
                "El ataque seleccionado es null."
            );

            return;
        }

        weapon = attack.GetComponentInChildren<RadialShotWeapon>(true);

        if (weapon == null)
        {
            Debug.LogWarning(
                $"El ataque '{attack.name}' " +
                $"no tiene un RadialShotWeapon."
            );

            return;
        }

        float duration = weapon.GetPatternDuration();

        Debug.Log(
            $"Activando ataque '{attack.name}' " +
            $"durante {duration} segundos."
        );

        attack.SetActive(true);

        attackCoroutine = boss.StartCoroutine(
            ExecuteAttack(duration)
        );
    }

    private IEnumerator ExecuteAttack(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (attack != null)
        {
            attack.SetActive(false);
        }

        attackCoroutine = null;

        boss.ChangeState(
            new BossPhaseState(
                boss,
                boss.CurrentPhase
            )
        );
    }

    public override void Exit()
    {
        if (attackCoroutine != null)
        {
            boss.StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        if (attack != null)
        {
            attack.SetActive(false);
        }

        Debug.Log(
            "Saliendo del estado de ataque."
        );
    }
}