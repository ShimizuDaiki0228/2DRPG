using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherIdleState : EnemyState
{

    private EnemyArcher enemy;

    public ArcherIdleState(Enemy _enemyBase, EnemyStateMachine _stateMchine, string _animBoolName, EnemyArcher _enemy) : base(_enemyBase, _stateMchine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.PlaySFX(14, enemy.transform);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangedState(enemy.moveState);

    }
}
