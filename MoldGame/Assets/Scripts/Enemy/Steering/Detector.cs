using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Detector : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;
    protected GameObject gameObject;

    public virtual void Initialize(GameObject gameObject, Enemy enemy)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.enemy = enemy;
    }

    public abstract void Detect(AIData aiData);

    public abstract void DrawGizmos();
}
