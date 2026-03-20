using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour, IDamageable, IManaHaver
{
    private Rigidbody2D RB;

    #region Spell Variables
    public ManaManager ManaManager { get; set; }

    [Header("Spells")]
    public SpellManager SpellManager;
    #endregion

    #region StateMachine Variables
    [Header("StateMachine Variables")]
    public PlayerStateManager StateManager;
    #endregion

    #region IDamageable Variables
    public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; } = 0f;
    public UnityEvent HealthChange;
    #endregion

    [SerializeField]
    private Animator[] _animators;
    private void Awake()
    {
        StateManager = GetComponentInChildren<PlayerStateManager>();
        ManaManager = GetComponentInChildren<ManaManager>();
        SpellManager = GetComponentInChildren<SpellManager>();        
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        HealthChange.Invoke();

        RB = GetComponent<Rigidbody2D>();
    }

    public void MovePlayer(Vector2 velocity)
    {
        RB.linearVelocity = velocity;
        UpdateAnimators(velocity);
    }   

    public void AnimationEventTrigger(AnimationTriggerType triggerType)
    {
        if (StateManager)
            StateManager.StateMachine.CurrentPlayerState.AnimationTriggerEvent(triggerType);
        else
            Debug.Log("State manager not set to an instance of an object");
    }

    #region IDamageable Functions
    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        HealthChange.Invoke();
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
    }

    public void Die()
    {
        print("Player died");
    }
    #endregion

    public enum AnimationTriggerType
    {
        PlayFootstep
    }

    private void UpdateAnimators(Vector2 velocity)
    {
        if(_animators.Length <= 0)
        {
            Debug.Log("No animators in play list");
            return;
        }
        foreach(Animator animator in _animators)
        {
            animator.SetFloat("XVelocity", velocity.normalized.x);
            animator.SetFloat("YVelocity", velocity.normalized.y);
        }
    }
}
