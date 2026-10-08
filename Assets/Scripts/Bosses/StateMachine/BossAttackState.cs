using UnityEngine;
using Bullets;

public class BossAttackState : BossState
{
    private GameObject attack;
    private BulletPatternWeapon weapon;

    public BossAttackState(
        BossBehavior boss,
        GameObject attack
    ) : base(boss)
    {
        this.attack = attack;
    }

    public override void Enter()
    {
        if (attack == null)
        {
            Debug.LogWarning(
                "BossAttackState: el ataque es null."
            );
            return;
        }

        weapon =
            attack.GetComponentInChildren<BulletPatternWeapon>(true);

        if (weapon == null)
        {
            Debug.LogWarning(
                $"No se encontró BulletPatternWeapon en {attack.name}."
            );
            return;
        }

        attack.SetActive(true);

        weapon.PatternFinished += OnPatternFinished;
        weapon.Play();
    }

    private void OnPatternFinished()
    {
        if (weapon != null)
            weapon.PatternFinished -= OnPatternFinished;

        if (attack != null)
            attack.SetActive(false);

        boss.ChangeState(
            new BossPhaseState(boss, boss.CurrentPhase)
        );
    }

    public override void Exit()
    {
        if (weapon != null)
        {
            weapon.PatternFinished -= OnPatternFinished;
            weapon.Stop();
        }

        if (attack != null)
            attack.SetActive(false);
    }
}