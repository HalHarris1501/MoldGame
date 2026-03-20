using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Detector-Target", menuName = "Detectors/Target")]
public class TargetDetector : Detector
{
    [SerializeField] private float _targetDetectionRange = 10f;

    [SerializeField] private LayerMask _obstacleLayerMask, _playerLayerMask;

    [SerializeField] private bool _showGizmos = false;
    [SerializeField] private float _gizmosSize = 0.3f;

    //gizmo parameters
    private List<Vector3> _colliderPositions;

    public override void Detect(AIData aiData)
    {
        //find out if player is near
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, _targetDetectionRange, _playerLayerMask);

        if (playerCollider == null)
        {
            _colliderPositions = null;
            enemy.SetAggroStatus(false);
            return;
        }
        //check if enemy can see the player
        Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _targetDetectionRange, _obstacleLayerMask);

        //Make sure the collider seen is on the "Player" layer
        if(hit.collider != null && (_playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
        {
            Debug.DrawRay(transform.position, direction * _targetDetectionRange, Color.magenta);
            _colliderPositions = new List<Vector3>() { playerCollider.transform.position };
            enemy.SetAggroStatus(true);            
        }
        else
        {
            _colliderPositions = null;
            enemy.SetAggroStatus(false);
        }

        aiData.targets = _colliderPositions;
    }

    public override void DrawGizmos()
    {
        if (!_showGizmos)
            return;

        Gizmos.DrawWireSphere(transform.position, _targetDetectionRange);

        if (_colliderPositions == null)
            return;
        Gizmos.color = Color.magenta;
        foreach (Vector3 itemTransform in _colliderPositions)
        {
            Gizmos.DrawSphere(itemTransform, _gizmosSize);
        }
    }
}
