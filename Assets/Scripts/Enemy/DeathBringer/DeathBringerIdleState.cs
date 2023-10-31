using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    private EnemyDeathBringer enemy;
    private Transform player;

    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMchine, string _animBoolName, EnemyDeathBringer _enemy) : base(_enemyBase, _stateMchine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(Vector2.Distance(player.transform.position, enemy.transform.position) < 10)
            enemy.bossFightBegun = true;

        //if (Input.GetKeyDown(KeyCode.Y))
        //    stateMachine.ChangedState(enemy.teleportState);

        if (stateTimer < 0 && enemy.bossFightBegun)
            stateMachine.ChangedState(enemy.battleState);

    }
}
