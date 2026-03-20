using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get //making sure that an instance always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectsByType<GameManager>(FindObjectsSortMode.None)[0];
            }
            return _instance;
        }
    }
    #endregion

    public PlayerController Player;
    public UnityEvent GameManagerInitialized;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Player = FindObjectsByType<PlayerController>(FindObjectsSortMode.None)[0];

        GameManagerInitialized.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
