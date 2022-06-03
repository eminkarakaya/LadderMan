using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _SceneManager : MonoBehaviour
{
    public int asynSceneIndex = 0;

    void Awake()
    {
        _SceneManager[] objs = GameObject.FindObjectsOfType<_SceneManager>();

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    
}
