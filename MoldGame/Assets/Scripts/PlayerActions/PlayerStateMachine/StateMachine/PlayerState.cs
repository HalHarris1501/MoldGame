using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : ScriptableObject
{
    protected PlayerController player;
    protected Transform transform;
    protected GameObject gameObject;
    protected PlayerStateMachine playerStateMachine;
    protected AudioSource audioSource;

    public virtual void Initialize(GameObject gameObject, PlayerController player, PlayerStateMachine playerStateMachine)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.audioSource = gameObject.GetComponent<AudioSource>();
    }

    public virtual void EnterState() 
    {
        InputManager.Instance.OnDash.AddListener(HandleDash);
    }
    public virtual void ExitState()
    {
        InputManager.Instance.OnDash.RemoveListener(HandleDash);
    }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent(PlayerController.AnimationTriggerType triggerType) { }

    public virtual void HandleDash() 
    {
        playerStateMachine.ChangeState(player.StateManager.DashState);
    }
}
