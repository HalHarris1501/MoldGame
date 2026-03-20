using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Steering Behaviour-Separation", menuName = "Steering Behaviours/Separation")]
public class SeparationBehaviour : SteeringBehaviour
{
    [SerializeField] private float _radius = 2f;

    [SerializeField] private bool _showGizmos = true;

    //gizmo Parameters
    float[] _dangersResultTemp = null;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        foreach (Collider2D allyCollider in aiData.allies)
        {
            Vector2 directionToAlly = allyCollider.ClosestPoint(transform.position) - (Vector2)transform.position;
            float distanceToAlly = directionToAlly.magnitude;

            //calculate weight base on the distance Enemy<--->Obstacle
            float weight = (_radius - distanceToAlly) / _radius;

            Vector2 directionToObstacleNormalized = directionToAlly.normalized;

            //Add obstacle parameters to the danger array
            for (int i = 0; i < Directions.eightDirections.Count; i++)
            {
                float dotProduct = Vector2.Dot(directionToObstacleNormalized, Directions.eightDirections[i]);

                float valueToPutIn = dotProduct * weight;

                //override value only if it is higher than the current one stored in the danger array
                if (valueToPutIn > danger[i])
                {
                    danger[i] = valueToPutIn;
                }
            }
        }
        _dangersResultTemp = danger;
        return (danger, interest);
    }

    public override void DrawGizmos()
    {
        if (!_showGizmos)
            return;

        if (Application.isPlaying && _dangersResultTemp != null)
        {
            if (_dangersResultTemp != null)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < _dangersResultTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * _dangersResultTemp[i]);
                }
            }
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
