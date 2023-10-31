using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroundedState : EnemyState
{
    protected Transform player;
    protected EnemySlime enemy;

    public SlimeGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMchine, string _animBoolName, EnemySlime _enemy) : base(_enemyBase, _stateMchine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.agroDistance)
        {
            stateMachine.ChangedState(enemy.battleState);
        }

    }
}
