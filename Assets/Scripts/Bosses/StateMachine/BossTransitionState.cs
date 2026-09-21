using System.Collections;
using UnityEngine;

public class BossTransitionState : BossState
{
    private int targetPhase;

    private Coroutine transitionCoroutine;

    public BossTransitionState(
        BossBehavior boss,
        int targetPhase) : base(boss)
    {
        this.targetPhase = targetPhase;
    }

    public override void Enter()
    {
        transitionCoroutine = boss.StartCoroutine(
            ExecuteTransition()
        );
    }

    private IEnumerator ExecuteTransition()
    {
        BossPhase phase = boss.GetPhaseData(targetPhase);

        if (phase.transitionEffect != null)
        {
            phase.transitionEffect.SetActive(true);
        }

        yield return new WaitForSeconds(
            phase.transitionPause
        );

        if (phase.transitionEffect != null)
        {
            phase.transitionEffect.SetActive(false);
        }

        boss.SetCurrentPhase(targetPhase);

        boss.ChangeState(
            new BossPhaseState(boss, targetPhase)
        );

        transitionCoroutine = null;
    }

    public override void Exit()
    {
        if (transitionCoroutine != null)
        {
            boss.StopCoroutine(transitionCoroutine);
            transitionCoroutine = null;
        }

        BossPhase phase = boss.GetPhaseData(targetPhase);

        if (phase.transitionEffect != null)
        {
            phase.transitionEffect.SetActive(false);
        }
    }
}