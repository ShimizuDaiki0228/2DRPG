using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyDeadState : EnemyState
{

    private EnemyShady enemy;

    public ShadyDeadState(Enemy _enemyBase, EnemyStateMachine _stateMchine, string _animBoolName, EnemyShady enemy) : base(_enemyBase, _stateMchine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
            enemy.SelfDestroy();
    }
}
