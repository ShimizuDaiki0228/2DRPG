using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathBringerTriggers : EnemyAnimationTriggers
{
    EnemyDeathBringer enemyDeathBringer => GetComponentInParent<EnemyDeathBringer>();

    private void Relocate() => enemyDeathBringer.FindPosition();

    private void MakeInvicible() => enemyDeathBringer.fx.MakeTransparent(true);
    private void MakeVicible() => enemyDeathBringer.fx.MakeTransparent(false);

}
