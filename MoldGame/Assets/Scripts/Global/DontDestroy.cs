using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [HideInInspector]
    public string ObjectID;

    private void Awake()
    {
        ObjectID = name + transform.position.ToString() + transform.eulerAngles.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < FindObjectsByType<DontDestroy>(FindObjectsSortMode.None).Length; i++)
        {
            if (FindObjectsByType<DontDestroy>(FindObjectsSortMode.None)[i] != this)
            {
                if (FindObjectsByType<DontDestroy>(FindObjectsSortMode.None)[i].ObjectID == ObjectID)
                {
                    Destroy(gameObject);
                }
            }
        }

        DontDestroyOnLoad(gameObject);
    }
}
