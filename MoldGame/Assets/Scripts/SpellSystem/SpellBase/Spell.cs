using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Spell-Default Spell", menuName = "Spells/Default")]
public class Spell : ScriptableObject
{
    [Header("Identification Variables")]
    public string SpellListName;
    public Sprite SpellSprite;
    [Header("Attacks")]
    [SerializeField]
    private AttackBase _quickCast;
    public AttackBase QuickCastInstance { get; set; }
    [SerializeField]
    private AttackBase _heldCast;
    public AttackBase HeldCastInstance { get; set; }

    private float _quickCooldown = 0;
    public float QuickCooldown
    {
        get { return _quickCooldown; }
        set { _quickCooldown = value; }
    }

    private float _heldCooldown = 0;
    public float HeldCooldown
    {
        get { return _heldCooldown; }
        set { _heldCooldown = value; }
    }

    protected float quickManaCost;
    protected float heldManaCost;
    protected IManaHaver owner;
    protected Transform transform;
    protected GameObject gameObject;
    protected AudioSource audioSource;

    public virtual void Initialize(GameObject gameObject, IManaHaver owner)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.owner = owner;
        this.audioSource = gameObject.GetComponentInChildren<AudioSource>();

        QuickCastInstance = Instantiate(_quickCast);
        HeldCastInstance = Instantiate(_heldCast);

        QuickCastInstance.Initialise(this, audioSource, owner, CooldownType.Quick);
        HeldCastInstance.Initialise(this, audioSource, owner, CooldownType.Held);
        
        quickManaCost = QuickCastInstance.ManaCost;
        heldManaCost = HeldCastInstance.ManaCost;
    }

    public virtual void TryHeldCast()
    {
        if (!CheckMana(heldManaCost))
            return;
        PerformHeldCast();
    }

    public virtual void PerformHeldCast()
    {
        if (_heldCooldown <= 0)
        {
            HeldCastInstance.Activate();
            owner.ManaManager.CurrentMana -= heldManaCost;
            owner.ManaManager.ManaChange.Invoke();
        }
    }

    public virtual void OnSpellStart()
    {
        
    }

    public virtual void OnSpellStop()
    {
        
    }

    public virtual void TryQuickCast()
    {
        if (!CheckMana(quickManaCost))
            return;
        PerformQuickCast();
    }

    public virtual void PerformQuickCast()
    {
        if (_quickCooldown <= 0)
        {
            QuickCastInstance.Activate();
            owner.ManaManager.CurrentMana -= quickManaCost;
            owner.ManaManager.ManaChange.Invoke();
        }
    }

    public virtual Quaternion RotationToMouse()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 1;
        Vector2 direction = direction = mousePos - owner.ManaManager.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        return rotation;
    }

    public virtual void OnUpdate()
    {
        _quickCooldown -= Time.deltaTime;
        _heldCooldown -= Time.deltaTime;
    }

    public virtual bool CheckMana(float castCost)
    {
        if (owner.ManaManager.CurrentMana >= castCost)
        {
            return true;
        }
        return false;
    }

    public virtual void SetCooldown(CooldownType cooldownType, float value)
    {
        if(cooldownType == CooldownType.Quick)
        {
            _quickCooldown = value;
        }
        else
        {
            _heldCooldown = value;
        }
    }
}

public enum CooldownType
{
    Quick,
    Held
}
