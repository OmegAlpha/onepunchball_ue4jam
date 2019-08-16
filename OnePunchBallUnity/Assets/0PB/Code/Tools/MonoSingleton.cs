using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : UnityEngine.Object
{
    protected static T instance;

    protected bool isInited = false;
	
    public bool IsInited
    {
        get { return isInited; }
    }

    protected void SetInited()
    {
        isInited = true;
    }
    
    public static T Get()
    {
        if (instance == null)
        {
            T[] instances = FindObjectsOfType<T>();
            if (instances.Length != 1)
            {
                throw new Exception("[MonoSingleton is wrong] 0 or multiple instances found of <"+ typeof(T) +">");
            }

            instance = instances[0];
        }

        

        return instance;
    }

    protected void OnDestroy()
    {
        instance = null;
    }
}
