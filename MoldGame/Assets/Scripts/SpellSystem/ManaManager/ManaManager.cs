using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ManaManager : MonoBehaviour
{
    public IManaHaver Owner;
    [Header("Mana")]
    [SerializeField]
    [Min(10f)]
    private float _maxMana;

    public float MaxMana => _maxMana;
    [SerializeField][Min(0.01f)][Tooltip("(X * MaxMana)/second")]
    private float _manaPercentageRegen;
    [SerializeField][Min(5f)][Tooltip("Current mana added to regen speed / X")]
    private float _MaxManaRegenBonusDenominator;
    public float CurrentMana;
    public UnityEvent ManaChange;

    // Start is called before the first frame update
    void Start()
    {
        Owner = GetComponentInParent<IManaHaver>();
        CurrentMana = _maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        RegenMana();
    }

    private void RegenMana()
    {
        if (CurrentMana > _maxMana)
        {
            CurrentMana = _maxMana;
        }
        else if (CurrentMana < _maxMana)
        {
            CurrentMana += ((_manaPercentageRegen * _maxMana) + _maxMana/_MaxManaRegenBonusDenominator) * Time.deltaTime;
            ManaChange.Invoke();
        }
    }
}
