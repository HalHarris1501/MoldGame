using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Move-Default Player Move", menuName = "Player States/Move State/Default Movement")]
public class DefaultMoveState : PlayerState
{
    [Header("Move Stats")]
    [SerializeField]
    private float _moveSpeed = 5f;

    [Header("Sound variables")]
    [SerializeField]
    private AudioClip _footstepSound;
    [SerializeField]
    private float _minPitch = 0.9f, _maxPitch = 1.3f;
    [SerializeField]
    private float _volume = 0.5f;

    private bool _isCasting = true;
    public override void AnimationTriggerEvent(PlayerController.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        if(triggerType == PlayerController.AnimationTriggerType.PlayFootstep)
        {
            PlayAudio();
        }
    }

    public override void EnterState()
    {
        base.EnterState();
        InputManager.Instance.QuickCast.performed += QuickCast;
        InputManager.Instance.HeldCast.performed += HoldingCast;
        InputManager.Instance.HeldCast.canceled += StoppedCast;
    }    

    public override void ExitState()
    {
        base.ExitState();
        InputManager.Instance.QuickCast.performed -= QuickCast;
        InputManager.Instance.HeldCast.performed -= HoldingCast;
        InputManager.Instance.HeldCast.canceled -= StoppedCast;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (_isCasting)
        {
            HeldCasting();
        }
    }

    public override void HandleDash()
    {
        base.HandleDash();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Vector2 moveDirection = InputManager.Instance.MoveAction.ReadValue<Vector2>().normalized;
        if (moveDirection.Abs() != Vector2.zero)
        {
            //PlayAudio();
        }

        player.MovePlayer(moveDirection * _moveSpeed);
    }
    private void HoldingCast(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        player.SpellManager.CastSpell(SpellManager.SpellCastType.Start);
        _isCasting= true;
    }

    private void StoppedCast(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (_isCasting)
        {
            player.SpellManager.CastSpell(SpellManager.SpellCastType.End);
        }
        _isCasting = false;
    }

    private void HeldCasting()
    {
        player.SpellManager.CastSpell(SpellManager.SpellCastType.Held);
    }

    private void QuickCast(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        player.SpellManager.CastSpell(SpellManager.SpellCastType.Quick);
    }

    private void PlayAudio()
    {
        if (!audioSource)
            return;

        audioSource.pitch = UnityEngine.Random.Range(_minPitch, _maxPitch);
        audioSource.PlayOneShot(_footstepSound, _volume);           
    }
}
