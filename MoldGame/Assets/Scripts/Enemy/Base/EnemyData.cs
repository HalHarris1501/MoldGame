using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public Stat MaxHealth;
    public Stat MaxSpeed;
    public Stat MaxMana;
}
