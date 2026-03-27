using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash-Default Player Dash", menuName = "Player States/Dash State/Default Dash")]
public class DefaultDashState : PlayerState
{
    [Header("Dash Stats")]
    [SerializeField]
    [Min(3f)]
    private float _dashDistance = 3f;
    [SerializeField]
    [Min(0.01f)]
    private float _dashDuration = 0.5f;

    [Header("Sound variables")]
    [SerializeField]
    private AudioClip _dashSound;
    [SerializeField]
    private float _minPitch = 0.9f, _maxPitch = 1.3f;

    private float _dashSpeed = 0;
    private Vector2 _moveVector;
    public override void AnimationTriggerEvent(PlayerController.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        _moveVector = InputManager.Instance.MoveAction.ReadValue<Vector2>().normalized;
        player.StartCoroutine(Dash());

        if (audioSource && _dashSound)
        {
            audioSource.pitch = UnityEngine.Random.Range(_minPitch, _maxPitch);
            audioSource.PlayOneShot(_dashSound);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void HandleDash()
    {
    }

    public override void Initialize(GameObject gameObject, PlayerController player, PlayerStateMachine playerStateMachine)
    {
        base.Initialize(gameObject, player, playerStateMachine);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.MovePlayer(_moveVector * _dashSpeed);
    }

    private void CalculateDashSpeed()
    {
        _dashSpeed = _dashDistance / _dashDuration;
    }

    public IEnumerator Dash()
    {        
        CalculateDashSpeed();
        if (_moveVector == Vector2.zero)
        {
            _moveVector = Vector2.up;
        }
        yield return new WaitForSeconds(_dashDuration);
        playerStateMachine.ChangeState(player.StateManager.MoveState);
    }
}
