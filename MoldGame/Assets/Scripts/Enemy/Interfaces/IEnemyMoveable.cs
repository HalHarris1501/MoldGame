using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody2D RB { get; set; }
    bool IsFacingRight { get; set; }

    void MoveEnemy();
    void CheckForFacingDirection(Vector2 velocity);
}
