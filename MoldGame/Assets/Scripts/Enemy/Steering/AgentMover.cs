using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    [SerializeField] private float _maxSpeed = 2f, _acceleration = 50f, _deacceleration = 100f;

    [SerializeField] private float _currentSpeed = 0f;

    private Vector2 _oldMovementInput;
    public Vector2 _movementInput { get; set; }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {        
        if(_movementInput.magnitude > 0 && _currentSpeed >= 0)
        {
            Debug.Log("My movement input: " + _movementInput.magnitude);
            _oldMovementInput = _movementInput;
            _currentSpeed += _acceleration * _maxSpeed * Time.deltaTime;
        }
        else
        {
            _currentSpeed -= _deacceleration * _maxSpeed * Time.deltaTime;
            
        }
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _maxSpeed);
        _rigidBody.linearVelocity = _oldMovementInput * _currentSpeed;
    }

    public void SetMaxSpeed(float maxSpeed)
    {
        _maxSpeed = maxSpeed;
    }
}
