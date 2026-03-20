using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;

[CreateAssetMenu(fileName = "Steering Behaviour-Seek", menuName = "Steering Behaviours/Seek")]
public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField] private float _targetReachedThreshold = 2f;

    [SerializeField] private bool _showGizmo = true;
    [SerializeField] private float _gizmosSize = 0.2f;

    bool reachedLastTarget = true;

    //gizmo parameters
    private Vector2 targetPositionCached;
    private float[] interestsTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {   
        if(reachedLastTarget)
        {
            //if we don't have a target stop seeking
            if (aiData.targets == null || aiData.targets.Count <= 0)
            {
                aiData.currentTarget = transform.position;
                return (danger, interest);
            }
            else //else set a new target
            {
                reachedLastTarget = false;
                aiData.currentTarget = aiData.targets.OrderBy(target => Vector2.Distance(target, transform.position)).First();
            }
        }

        //cache the last position only if we still see the target (if the targets collection is not empty)
        if (aiData.currentTarget != transform.position && aiData.targets != null)
        {
            if (aiData.targets.Count > 0)
            {
                aiData.currentTarget = aiData.targets.OrderBy(target => Vector2.Distance(target, transform.position)).First();
            }
            else
            {
                targetPositionCached = aiData.currentTarget;
            }
        }


        if (aiData.currentTarget == null || aiData.currentTarget == transform.position)
        {
            //check if we have reached the target
            if (Vector2.Distance(transform.position, targetPositionCached) < _targetReachedThreshold)
            {
                reachedLastTarget = true;
                aiData.currentTarget = transform.position;
                return (danger, interest);
            }

            //if we havent reached the target yet, do the main logic of finding the interest direcions
            Vector2 directionToTarget = (targetPositionCached - (Vector2)transform.position);
            for (int i = 0; i < interest.Length; i++)
            {
                //get dot product for current direction
                float dotProduct = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);

                //accept only directions at the less than 90 degrees to the target direction
                if (dotProduct > 0)
                {
                    float valueToPutIn = dotProduct;
                    if (valueToPutIn > interest[i])
                    {
                        interest[i] = valueToPutIn;
                    }
                }
            }
        }
        else
        {
            //check if we have reached the target
            if (Vector2.Distance(transform.position, aiData.currentTarget) < _targetReachedThreshold)
            {
                reachedLastTarget = true;
                aiData.currentTarget = transform.position;
                return (danger, interest);
            }

            //if we havent reached the target yet, do the main logic of finding the interest direcions
            Vector2 directionToTarget = ((Vector2)aiData.currentTarget - (Vector2)transform.position);
            for (int i = 0; i < interest.Length; i++)
            {
                //get dot product for current direction
                float dotProduct = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);

                //accept only directions at the less than 90 degrees to the target direction
                if (dotProduct > 0)
                {
                    float valueToPutIn = dotProduct;
                    if (valueToPutIn > interest[i])
                    {
                        interest[i] = valueToPutIn;
                    }
                }
            }
        }
        interestsTemp = interest;
        return (danger, interest);
    }

    public override void DrawGizmos()
    {
        if (!_showGizmo)
            return;
        Gizmos.DrawSphere(targetPositionCached, _gizmosSize);

        if(Application.isPlaying && interestsTemp != null)
        {
            if(interestsTemp != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < interestsTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i]);
                }
                if(!reachedLastTarget)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(targetPositionCached, _gizmosSize / 2);
                }
            }
        }
    }
}
