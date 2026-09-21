public abstract class BossState
{
    protected BossBehavior boss;

    protected BossState(BossBehavior boss)
    {
        this.boss = boss;
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void Update()
    {
    }
}