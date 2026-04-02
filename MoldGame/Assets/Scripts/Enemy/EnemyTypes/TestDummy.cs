using UnityEngine;

public class TestDummy : MonoBehaviour, IDamageable
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        Debug.Log("Oof: " + damageAmount);
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
}
