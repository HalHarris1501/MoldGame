using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseSOBase : EnemyStateSOBase
{
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (enemy.IsWithinStrikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }
    }
}
