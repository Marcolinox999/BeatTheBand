using System.Collections;
using UnityEngine;
using Bullets;

public class BossAttackState : BossState
{
    private GameObject attack;
    private BulletPatternWeapon weapon;

    [Header("Vinyl")] 
    private float vinylAttackCD = 30f;

    public BossAttackState(BossBehavior boss, GameObject attack) : base(boss)
    {
        this.attack = attack;
    }

    public override void Enter()
    {
        if (attack == null)
        {
            EndAttack();
            return;
        }

        attack.SetActive(true);

        // 1. Intentamos disparar balas
        weapon = attack.GetComponentInChildren<BulletPatternWeapon>(true);
        if (weapon != null)
        {
            weapon.PatternFinished += OnPatternFinished;
            weapon.Play();
            return; 
        }
            
        // 2. Intentamos hacer el Vinyl
        VinylScratch vinylScratch = attack.GetComponentInChildren<VinylScratch>(true);
        if (vinylScratch != null)
        {
            vinylScratch.TriggerVinylScratch();
            boss.StartCoroutine(WaitAndFinish(vinylAttackCD));
            return; 
        }
            
        EndAttack();
    }
    
    private IEnumerator WaitAndFinish(float time)
    {
        yield return new WaitForSeconds(time);
        EndAttack();
    }

    private void OnPatternFinished()
    {
        if (weapon != null)
            weapon.PatternFinished -= OnPatternFinished;

        EndAttack();
    }
    
    private void EndAttack()
    {
        if (attack != null)
            attack.SetActive(false);

        boss.ChangeState(new BossPhaseState(boss, boss.CurrentPhase));
    }

    public override void Exit()
    {
        if (weapon != null)
        {
            weapon.PatternFinished -= OnPatternFinished;
            weapon.Stop();
        }
        if (attack != null) attack.SetActive(false);
    }
}