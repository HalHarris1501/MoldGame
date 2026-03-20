using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Singleton
    private static InputManager _instance;
    public static InputManager Instance
    {
        get //making sure that an instance always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectsByType<InputManager>(FindObjectsSortMode.None)[0];
            }
            return _instance;
        }
    }
    #endregion

    [HideInInspector]
    public InputAction MoveAction, DashAction, QuickCast, HeldCast, QuickSwapSpell;
    [HideInInspector]
    public UnityEvent OnDash;
    [HideInInspector]
    public UnityEvent InputManagerInitialized;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        MoveAction = InputSystem.actions.FindAction("Player/Move");
        DashAction = InputSystem.actions.FindAction("Player/Dash");
        QuickCast = InputSystem.actions.FindAction("Player/QuickCastSpell");
        HeldCast = InputSystem.actions.FindAction("Player/HeldCastSpell");
        QuickSwapSpell = InputSystem.actions.FindAction("Player/QuickSwapSpell");

        DashAction.performed += ctx => OnDash.Invoke();
        InputManagerInitialized.Invoke();
        Debug.Log("Input manager initialized");
    }    

    private void OnEnable()
    {
        InputSystem.actions.FindActionMap("Player").Enable();
    }    

    private void OnDisable()
    {
        InputSystem.actions.FindActionMap("Player").Disable();
    }

    private void TestAction()
    {
        Debug.Log("Action Working");
    }
}
