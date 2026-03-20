using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;
    [HideInInspector]
    public ManaManager PlayerMana;
    private float _maxPlayerMana;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance)
        {
            PlayerMana = GameManager.Instance.Player.ManaManager;
            PlayerMana.ManaChange.AddListener(SetManaValue);
            _maxPlayerMana = PlayerMana.MaxMana;
            SetManaValue();
        }
        else
            Debug.Log("Game manager not set to an instance of an object");
    }

    private void OnDisable()
    {
        if (PlayerMana)
            PlayerMana.ManaChange.RemoveListener(SetManaValue);
        else
            Debug.Log("PlayerMana not set to an instance of an object");
    }

    private void SetManaValue()
    {
        if (_slider != null)
            _slider.value = PlayerMana.CurrentMana / _maxPlayerMana;
        else
            Debug.Log("_slider not set to an instance of an object");
    }
}
