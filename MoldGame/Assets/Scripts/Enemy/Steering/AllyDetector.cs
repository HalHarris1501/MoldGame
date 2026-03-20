using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Detector-Ally", menuName = "Detectors/Ally")]
public class AllyDetector : Detector
{
    [SerializeField] private float _detetctionRadius = 2;

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private bool _showGizmos = true;
    [SerializeField] private float _gizmoSize = 0.2f;

    Collider2D[] _colliders;

    public override void Detect(AIData aiData)
    {
        _colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, _detetctionRadius, _layerMask);
        //int colliderToRemove = -1;
        //for (int i = 0; i < _colliders.Length; i++)
        //{
        //    if (_colliders[i].gameObject == this.gameObject)
        //    {
        //        colliderToRemove = i;
        //    }
        //}
        //if (colliderToRemove != -1 && colliderToRemove <= _colliders.Length)
        //{
        //    _colliders = RemoveAt(ref _colliders, colliderToRemove);
        //}
        aiData.allies = _colliders;
    }

    public static void RemoveAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            // moving elements downwards, to fill the gap at [index]
            arr[a] = arr[a + 1];
        }
        // decrement Array's size by one
        Array.Resize(ref arr, arr.Length - 1);
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
