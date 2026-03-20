using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Steering Behaviour-Obstacle Avoidance", menuName = "Steering Behaviours/Obstacle Avoidance")]
public class ObstacleAvoidanceBehaviour : SteeringBehaviour
{
    [SerializeField] private float _radius = 2f, agentColliderSize = 0.6f;

    [SerializeField] private bool _showGizmos = true;

    //gizmo Parameters
    float[] _dangersResultTemp = null;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        foreach (Collider2D obstacleCollider in aiData.obstacles)
        {
            Vector2 directionToObstacle = obstacleCollider.ClosestPoint(transform.position) - (Vector2)transform.position;
            float distanceToObstacle = directionToObstacle.magnitude;

            //calculate weight base on the distance Enemy<--->Obstacle
            float weight = distanceToObstacle <= agentColliderSize ? 1 : (_radius - distanceToObstacle) / _radius;

            Vector2 directionToObstacleNormalized = directionToObstacle.normalized;

            //Add obstacle parameters to the danger array
            for (int i = 0; i < Directions.eightDirections.Count; i++)
            {
                float dotProduct = Vector2.Dot(directionToObstacleNormalized, Directions.eightDirections[i]);

                float valueToPutIn = dotProduct * weight;

                //override value only if it is higher than the current one stored in the danger array
                if(valueToPutIn > danger[i])
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
        if(!_showGizmos)
            return;

        if(Application.isPlaying && _dangersResultTemp != null)
        {
            if(_dangersResultTemp != null)
            {
                Gizmos.color = Color.red;
                for(int i = 0; i < _dangersResultTemp.Length; i++)
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

public static class Directions
{
    public static List<Vector2> eightDirections = new List<Vector2> {
        new Vector2(0,1).normalized,
        new Vector2(1,1).normalized,
        new Vector2(1,0).normalized,
        new Vector2(1,-1).normalized,
        new Vector2(0,-1).normalized,
        new Vector2(-1,-1).normalized,
        new Vector2(-1,0).normalized,
        new Vector2(-1,1).normalized,
    };
}
