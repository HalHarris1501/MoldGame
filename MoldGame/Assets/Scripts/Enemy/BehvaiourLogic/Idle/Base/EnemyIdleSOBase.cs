using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleSOBase : EnemyStateSOBase
{
    public override void DoFrameUpdateLogic() 
    {
        base.DoFrameUpdateLogic();

        if (enemy.IsAggroed)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }
    }
}
