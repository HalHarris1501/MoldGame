using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSolver
{
    [SerializeField] private bool _showGizmos = true;

    //gizmo parameters
    float[] _interestGizmo = new float[0];
    Vector2 _resultDirection = Vector2.zero;
    private float _rayLength = 1f;
    private Transform _transform;

    public void Initialize(GameObject gameObject)
    {
        _interestGizmo = new float[8];
        _transform = gameObject.transform;
    }

    public Vector2 GetDirectionToMove(List<SteeringBehaviour> behaviours, AIData aiData)
    {
        float[] danger = new float[8];
        float[] interest = new float[8];

        //loop through each behaviour
        foreach (SteeringBehaviour behaviour in behaviours)
        {
            (danger, interest) = behaviour.GetSteering(danger, interest, aiData);
        }

        //subtract danger values from interest array
        for (int i = 0; i < 8; i++)
        {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }

        _interestGizmo = interest;

        //get the average direction
        Vector2 outputDirection = Vector2.zero;
        for(int i = 0; i < 8; i++)
        {
            outputDirection += Directions.eightDirections[i] * interest[i];
        }
        outputDirection.Normalize();

        _resultDirection = outputDirection;

        return _resultDirection;
    }

    private void OnDrawGizmos()
    {
        if(Application.isPlaying && _showGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(_transform.position, _resultDirection * _rayLength);
        }
    }
}
