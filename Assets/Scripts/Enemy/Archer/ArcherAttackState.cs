using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttackState : EnemyState
{
    EnemyArcher enemy;

    public ArcherAttackState(Enemy _enemyBase, EnemyStateMachine _stateMchine, string _animBoolName, EnemyArcher enemy) : base(_enemyBase, _stateMchine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangedState(enemy.battleState);
    }
}
