using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Detector-Obstacle", menuName = "Detectors/Obstacle")]
public class ObstacleDetector : Detector
{
    [SerializeField] private float _detetctionRadius = 2;

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private bool _showGizmos = true;
    [SerializeField] private float _gizmoSize = 0.2f;

    Collider2D[] _colliders;

    public override void Detect(AIData aiData)
    {
        _colliders = Physics2D.OverlapCircleAll(transform.position, _detetctionRadius, _layerMask);
        aiData.obstacles = _colliders;
    }

    public override void DrawGizmos()
    {
        if (!_showGizmos)
            return;
        if (_colliders != null && Application.isPlaying)
        {
            Gizmos.color = Color.red;
            foreach (Collider2D obstacleCollider in _colliders)
            {
                Gizmos.DrawSphere(obstacleCollider.transform.position, _gizmoSize);
            }
        }
    }
}
