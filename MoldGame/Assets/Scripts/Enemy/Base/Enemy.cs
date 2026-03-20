using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable, IManaHaver
{

    [Header("Stats")][SerializeField]
    private EnemyData _enemyData;
    public EnemyData EnemyDataInstance { get; set; }

    #region Attack Variables
    [SerializeField] private Spell _spell;
    public Spell SpellInstance { get; set; }
    #endregion

    public float MaxHealth
    { 
        get { return EnemyDataInstance.MaxHealth.CurrentValue; }
        set { EnemyDataInstance.MaxHealth.CurrentValue = value; }
    }
    public float CurrentHealth { get; set; }
    public Rigidbody2D RB { get; set; }
    public bool IsFacingRight { get; set; } = true;

    #region ITriggerCheckable Variables
    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }
    #endregion

    #region State Machine Variables

    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }

    #endregion

    #region ScriptableObject State Variables
    [Header("State Machine Variables")]
    [SerializeField]
    private EnemyIdleSOBase _enemyIdleBase;
    [SerializeField]
    private EnemyChaseSOBase _enemyChaseBase;
    [SerializeField]
    private EnemyAttackSOBase _enemyAttackBase;

    public EnemyIdleSOBase EnemyIdleBaseInstance { get; set; }
    public EnemyChaseSOBase EnemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase EnemyAttackBaseInstance { get; set; }

    #endregion

    #region Steering Behvaiours Variables
    [Header("Steering Behaviour Variables")]
    [SerializeField] private List<SteeringBehaviour> _steeringBehaviours;
    [SerializeField] private List<Detector> _detectors;

    [HideInInspector]public List<SteeringBehaviour> SteeringBehavioursInstance = new List<SteeringBehaviour>();
    [HideInInspector]public List<Detector> DetectorsInstance = new List<Detector>();

    public AIData AiData;

    [SerializeField] private float _detectionDelay = 0.05f;

    private Vector2 _movementInput;

    public ContextSolver MovementDirectionSolver;

    private AgentMover _agentMover;
    #endregion

    #region Mana Manager Variables
    public ManaManager ManaManager { get; set; }
    #endregion

    private void Awake()
    {
        //enemy data
        if (_enemyData != null)
        {
            EnemyDataInstance = Instantiate(_enemyData);
        }
        else
        {
            EnemyDataInstance = null;
            Debug.Log("_enemyData is null");
        }
        if (_spell != null)
        {
            SpellInstance = Instantiate(_spell);
        }
        else
        {
            SpellInstance = null;
            Debug.Log("_spell is null");
        }

        //state machine
        if (_enemyIdleBase != null)
        {
            EnemyIdleBaseInstance = Instantiate(_enemyIdleBase);
        }
        else
        {
            EnemyIdleBaseInstance = null;
            Debug.Log("_enemyIdleBase is null");
        }
        if (_enemyChaseBase != null)
        {
            EnemyChaseBaseInstance = Instantiate(_enemyChaseBase);
        }
        else
        {
            EnemyChaseBaseInstance = null;
            Debug.Log("_enemyChaseBase is null");
        }
        if (_enemyAttackBase != null)
        {
            EnemyAttackBaseInstance = Instantiate(_enemyAttackBase);
        }
        else
        {
            EnemyAttackBaseInstance = null;
            Debug.Log("_enemyAttackBase is null");
        }

        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);

        //context steering
        foreach(SteeringBehaviour behaviour in _steeringBehaviours)
        {
            SteeringBehaviour behaviourInstance = Instantiate(behaviour);
            SteeringBehavioursInstance.Add(behaviourInstance);
        }

        foreach(Detector detector in _detectors)
        {
            Detector detectorInstance = Instantiate(detector);
            DetectorsInstance.Add(detectorInstance);
        }

        MovementDirectionSolver = new ContextSolver();
        AiData = new AIData();
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;

        RB = GetComponent<Rigidbody2D>();

        _agentMover = GetComponent<AgentMover>();
        if(SpellInstance != null)
        {
            SpellInstance.Initialize(this.gameObject, this);
        }
        else
        {
            Debug.Log("SpellInstance is null");
        }
        if (EnemyDataInstance != null)
        {
            _agentMover.SetMaxSpeed(EnemyDataInstance.MaxSpeed.CurrentValue);
        }
        else
        {
            Debug.Log("EnemyDataInstance is null");
        }

        if (EnemyIdleBaseInstance != null)
        {
            EnemyIdleBaseInstance.Initialize(gameObject, this);
        }
        else
        {
            Debug.Log("EnemyIdleBaseInstance is null");
        }
        if (EnemyChaseBaseInstance != null)
        {
            EnemyChaseBaseInstance.Initialize(gameObject, this);
        }
        else
        {
            Debug.Log("EnemyChaseBaseInstance is null");
        }
        if (EnemyAttackBaseInstance != null)
        {
            EnemyAttackBaseInstance.Initialize(gameObject, this);
        }
        else
        {
            Debug.Log("EnemyAttackBaseInstance is null");
        }

        StateMachine.Initialize(IdleState);

        MovementDirectionSolver.Initialize(gameObject);

        foreach (SteeringBehaviour behaviour in SteeringBehavioursInstance)
        {
            behaviour.Initialize(gameObject, this);
        }

        foreach (Detector detector in DetectorsInstance)
        {
            detector.Initialize(gameObject, this);
        }

        InvokeRepeating(nameof(PerformDetection), 0, _detectionDelay);
    }

    private void PerformDetection()
    {
        foreach (Detector detector in DetectorsInstance)
        {
            detector.Detect(AiData);
        }
    }

    private void Update()
    {        
        StateMachine.CurrentEnemyState.FrameUpdate();
        if (SpellInstance != null)
        {
            SpellInstance.OnUpdate();
        }
        else
        {
            Debug.Log("SpellInstance is null");
        }
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region IDamageable Functions
    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log(gameObject.name + " is dead");
    }
    #endregion

    #region IEnemyMoveable Functions
    public void MoveEnemy()
    {
        if (_agentMover)
        {
            _agentMover._movementInput = MovementDirectionSolver.GetDirectionToMove(SteeringBehavioursInstance, AiData);
            CheckForFacingDirection(RB.linearVelocity);
        }
        else
        {
            Debug.Log("Agent Mover not set for " + gameObject.name);
        }
    }

    public void StopEnemy()
    {
        if(_agentMover)
        {
            _agentMover._movementInput = Vector2.zero;
        }
    }

    public void CheckForFacingDirection(Vector2 velocity)
    {
        if (IsFacingRight && velocity.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
        else if (!IsFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
    }
    #endregion

    #region ITriggerCheckable Functions
    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }

    public void SetStrikingDistanceStatus(bool isWithinStrikingDistance)
    {
        IsWithinStrikingDistance = isWithinStrikingDistance;
    }
    #endregion

    #region Animation Triggers

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootstepSound
    }

    #endregion

    private void OnDrawGizmos()
    {
        foreach(SteeringBehaviour behaviour in SteeringBehavioursInstance)
        {
            behaviour.DrawGizmos();
        }
        foreach(Detector detector in DetectorsInstance)
        {
            detector.DrawGizmos();
        }
    }
}
