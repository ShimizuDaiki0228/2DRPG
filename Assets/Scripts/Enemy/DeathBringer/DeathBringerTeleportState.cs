using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerTeleportState : EnemyState
{

    private EnemyDeathBringer enemy;

    public DeathBringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMchine, string _animBoolName, EnemyDeathBringer enemy) : base(_enemyBase, _stateMchine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.stats.MakeInvincible(true);

    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            if(enemy.CanDoSpellCast())
                stateMachine.ChangedState(enemy.spellCastState);
            else
                stateMachine.ChangedState(enemy.battleState);

        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stats.MakeInvincible(false);
    }
}
