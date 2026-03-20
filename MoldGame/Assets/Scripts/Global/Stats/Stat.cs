using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private float _baseValue;
    public float BaseValue
    {
        get { return _baseValue; }
        set { _baseValue = value; }
    }
    [SerializeField]
    private float _currentValue;
    public float CurrentValue
    {
        get { return CalculateCurrentValue(); }
        set { _currentValue = value; }
    }
    [SerializeField]
    private float[] _bonus;
    public float[] Bonus
    { 
        get { return _bonus; }
        set { _bonus = value; }
    }
    [SerializeField]
    private float[] _multipliers;
    public float[] Mulipliers
    {
        get { return _multipliers; }
        set { _multipliers = value; }
    }
    [SerializeField]
    private float[] _flatBonus;
    public float[] FlatBonus
    {
        get { return _flatBonus; }
        set { _flatBonus = value; }
    }

    private float CalculateCurrentValue()
    {
        _currentValue = _baseValue;
        foreach(float bonus in _bonus)
        {
            _currentValue += bonus;
        }
        foreach(float mult in _multipliers)
        {
            _currentValue *= mult;
        }
        foreach (float flat in _flatBonus)
        {
            _currentValue += flat;
        }

        return _currentValue;
    }
}
