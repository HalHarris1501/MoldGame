using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [Header("Gameplay Variables")]
    [SerializeField] 
    private float _moveSpeed = 10f;
    [SerializeField][Tooltip("In seconds")]
    private float _duration = 1f;
    [SerializeField][Min(0.1f)]
    private float _damage = 1f;
    [SerializeField]
    private LayerMask _collidables;

    [Header("Sound Variables")]
    [SerializeField]
    private float _volume = 0.1f;
    [SerializeField]
    private AudioClip _spawnSound;
    [SerializeField]
    private AudioClip _destroySound;

    private Rigidbody2D _rb;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_spawnSound)
        {
            AudioSource.PlayClipAtPoint(_spawnSound, transform.position, _volume);
        }
        StartCoroutine(Duration());
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = transform.up * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _collidables) != 0)
        {
            HitObject(collision);
        }
    }

    private IEnumerator Duration()
    {
        yield return new WaitForSeconds(_duration);
        Deactivate();
    }

    private void Deactivate()
    {
        if (_destroySound)
        {
            AudioSource.PlayClipAtPoint(_destroySound, transform.position);
        }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    public float GetRange()
    {
        float range = _duration * _moveSpeed;
        return range;
    }

    private void HitObject(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(_damage);
        }
        Deactivate();
    }
}
