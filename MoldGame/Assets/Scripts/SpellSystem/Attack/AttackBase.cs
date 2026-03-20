using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class AttackBase : ScriptableObject
{
    [Header("Attack Stats")]
    [SerializeField] private float _shotsPerSecond = 1f;
    public float ShotsPerSecond => _shotsPerSecond;
    [SerializeField] private ProjectileBase _projectile;
    public ProjectileBase Projectile => _projectile;
    [SerializeField] private float _manaCost;
    public float ManaCost => _manaCost;

    [Header("Sound Variables")]
    [SerializeField]
    private AudioClip _castSound;
    [SerializeField]
    private float _volume = 0.5f;

    protected Spell _mySpell;
    protected float _fireRate;
    protected AudioSource audioSource;
    protected IManaHaver owner;
    protected CooldownType cooldownType;
    public virtual void Initialise(Spell spell, AudioSource audioSource, IManaHaver owner, CooldownType type)
    {
        _mySpell = spell;
        _fireRate = 1 / _shotsPerSecond;
        this.audioSource = audioSource;
        this.owner = owner;
        cooldownType = type;
    }

    public virtual void Activate()
    {
        _mySpell.SetCooldown(cooldownType, _fireRate);
        if (_castSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(_castSound, _volume);
        }
    }
}
