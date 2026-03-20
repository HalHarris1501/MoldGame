using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack-Projectile-Default Attack Projectile", menuName = "Attacks/Projectile Attack/Default")]
public class AttackSpawnProjectile : AttackBase
{
    public override void Activate()
    {
        base.Activate();
        if(Projectile == null)
        {
            Debug.Log("Projectile is null");
            return;
        }
        if(owner == null)
        {
            Debug.Log("Owner is null");
            return;
        }
        if(owner.ManaManager == null)
        {
            Debug.Log("ManaManager is null");
            return;
        }
        if(_mySpell == null)
        {
            Debug.Log("_mySpell is null");
            return;
        }
        ObjectPoolManager.SpawnObject(Projectile, owner.ManaManager.transform.position, _mySpell.RotationToMouse());
    }
}
