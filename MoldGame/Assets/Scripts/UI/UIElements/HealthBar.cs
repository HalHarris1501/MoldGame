using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;
    [HideInInspector]
    public PlayerController PlayerHealth;
    private float _maxPlayerHealth;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance)
        {
            PlayerHealth = GameManager.Instance.Player;
            PlayerHealth.HealthChange.AddListener(SetHealthValue);
            _maxPlayerHealth = PlayerHealth.MaxHealth;
            SetHealthValue();
        }
        else
            Debug.Log("Game manager not set to an instance of an object");
    }

    private void OnDisable()
    {
        if (PlayerHealth)
            PlayerHealth.HealthChange.RemoveListener(SetHealthValue);
        else
            Debug.Log("PlayerMana not set to an instance of an object");
    }

    private void SetHealthValue()
    {
        if (_slider != null)
            _slider.value = PlayerHealth.CurrentHealth / _maxPlayerHealth;
        else
            Debug.Log("_slider not set to an instance of an object");
    }
}
