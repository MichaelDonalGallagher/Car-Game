using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance;

    public static Manager Instance { get { return _instance; } }

    public Dictionary<string, GameObject> markers;
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            markers = new Dictionary<string, GameObject>();
        }
    }
}
