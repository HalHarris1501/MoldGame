using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GlobalAudioSource : MonoBehaviour
{
    #region Singleton
    private static GlobalAudioSource _instance;
    public static GlobalAudioSource Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectsByType<GlobalAudioSource>(FindObjectsSortMode.None)[0];
            }
            return _instance;
        }
    }
    #endregion

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
    }

    public AudioSource GlobalSource;
    void Start()
    {
        GlobalSource = GetComponent<AudioSource>();
    }
}
