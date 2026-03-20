using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSwapSpellUI : MonoBehaviour
{
    [SerializeField]
    private Image _currentSpellImage;
    [SerializeField]
    private Image _quickSwapSpellImage;
    private SpellManager _playerSpellManager;

    private void Start()
    {
        if (GameManager.Instance)
        {
            _playerSpellManager = GameManager.Instance.Player.SpellManager;
            _playerSpellManager.SpellChange.AddListener(SetSpellImages);
            SetSpellImages();
        }
    }

    private void OnDisable()
    {
        if (_playerSpellManager && GameManager.Instance)
        {
            _playerSpellManager = GameManager.Instance.Player.SpellManager;
            _playerSpellManager.SpellChange.RemoveListener(SetSpellImages);
        }
    }

    private void SetSpellImages()
    {
        if (_currentSpellImage)
            _currentSpellImage.sprite = _playerSpellManager.GetSpells()[0].SpellSprite;
        if (_quickSwapSpellImage)
            _quickSwapSpellImage.sprite = _playerSpellManager.GetSpells()[1].SpellSprite;
    }
}
