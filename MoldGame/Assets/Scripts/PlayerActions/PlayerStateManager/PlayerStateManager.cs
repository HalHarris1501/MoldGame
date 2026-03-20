using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerController Player;
    public PlayerStateMachine StateMachine;
    [Header("States")]
    [SerializeField]
    private PlayerState _moveState;
    [SerializeField]
    private PlayerState _dashState;

    public PlayerState MoveState { get; set; }
    public PlayerState DashState { get; set; }

    private void Awake()
    {
        Player = GetComponentInParent<PlayerController>();

        if (_moveState)
            MoveState = Instantiate(_moveState);
        else
            Debug.Log("Move state not set to an instance of an object");
        if (_dashState)
            DashState = Instantiate(_dashState);
        else
            Debug.Log("Dash state not set to an instance of an object");          

        StateMachine = new PlayerStateMachine();
    }

    private void Start()
    {
        if (Player)
        {
            MoveState.Initialize(Player.gameObject, Player, StateMachine);
            DashState.Initialize(Player.gameObject, Player, StateMachine);

            StateMachine.Initialize(MoveState);
        }
        else
            Debug.Log("Player not set to an instance of an object");
    }

    private void Update()
    {
        if (StateMachine.CurrentPlayerState)
            StateMachine.CurrentPlayerState.FrameUpdate();
        else
            Debug.Log("StateMachine CurrentPlayerState not set to an instance of an object");
    }

    private void FixedUpdate()
    {
        if (StateMachine.CurrentPlayerState)
            StateMachine.CurrentPlayerState.PhysicsUpdate();
        else
            Debug.Log("StateMachine CurrentPlayerState not set to an instance of an object");
    }
}
