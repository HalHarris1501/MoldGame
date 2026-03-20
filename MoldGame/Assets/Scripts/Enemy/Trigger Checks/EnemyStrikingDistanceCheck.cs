using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrikingDistanceCheck : MonoBehaviour
{
    public GameObject PlayerTarget { get; set; }
    private Enemy _enemy;
    private CircleCollider2D _collider;
    [SerializeField] private float _defaultRadius = 3f;

    private void Awake()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("PlayerCollider");
        _collider = GetComponent<CircleCollider2D>();

        _enemy = GetComponentInParent<Enemy>();
        _collider.radius = _defaultRadius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerTarget)
        {
            _enemy.SetStrikingDistanceStatus(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerTarget)
        {
            _enemy.SetStrikingDistanceStatus(false);
        }
    }
}
