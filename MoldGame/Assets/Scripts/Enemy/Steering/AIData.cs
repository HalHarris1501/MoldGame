using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData
{
    public List<Vector3> targets = new List<Vector3>();
    public Collider2D[] obstacles = null;
    public Collider2D[] allies = null;

    public Vector3 currentTarget;

    public int GetTargetsCount() => targets == null ? 0 : targets.Count;
}
