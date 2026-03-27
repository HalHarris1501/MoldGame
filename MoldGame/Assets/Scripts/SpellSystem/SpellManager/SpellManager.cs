using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellManager : MonoBehaviour
{
    public PlayerController Player;
    [Header("Spells")]
    private Spell _currentSpell;
    private Spell _quickSwapSpell;
    [SerializeField]
    private Spell[] _spells;

    public UnityEvent SpellChange;
    public Dictionary<string, Spell> Spells;

    private void Awake()
    {
        Spells = new Dictionary<string, Spell>();
        for (int i = 0; i < _spells.Length; i++)
        {
            Spells.Add(_spells[i].SpellListName, Instantiate(_spells[i]));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (InputManager.Instance)
        {
            SetupControls();
        }

        Player = GetComponentInParent<PlayerController>();
        foreach (Spell spell in Spells.Values)
        {
            Spells[spell.SpellListName].Initialize(Player.gameObject, Player);
        }
        if (Spells.Count > 0)
        {
            _currentSpell = Spells[_spells[0].SpellListName];
        }
        if (Spells.Count > 1)
        {
            _quickSwapSpell = Spells[_spells[1].SpellListName];
        }
        SpellChange?.Invoke();
    }

    private void OnEnable()
    {
        if (InputManager.Instance)
        {
            InputManager.Instance.InputManagerInitialized.AddListener(SetupControls);
        }
        else
            Debug.Log("No Input Manager found");
    }

    private void SetupControls()
    {
        InputManager.Instance.QuickSwapSpell.performed += QuickSwapSpell;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.QuickSwapSpell.performed -= QuickSwapSpell;
            InputManager.Instance.InputManagerInitialized.RemoveListener(SetupControls);
        }            
    }

    public void CastSpell(SpellCastType castType)
    {
        if (castType == SpellCastType.Quick)
        {
            _currentSpell.TryQuickCast();
        }
        if (castType == SpellCastType.Held)
        {
            _currentSpell.TryHeldCast();
        }
        if (castType == SpellCastType.Start)
        {
            _currentSpell.OnSpellStart();
        }
        if (castType == SpellCastType.End)
        {
            _currentSpell.OnSpellStop();
        }
    }

    public enum SpellCastType
    {
        Quick,
        Held,
        Start,
        End
    }

    private void QuickSwapSpell(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_quickSwapSpell)
        {
            Spell temp = _currentSpell;
            _currentSpell = _quickSwapSpell;
            _quickSwapSpell = temp;
            SpellChange.Invoke();            
        }
    }

    public Spell[] GetSpells()
    {
        Spell[] temp = { _currentSpell, _quickSwapSpell };
        return temp;
    }

    private void Update()
    {
        _currentSpell.OnUpdate();
    }
}
