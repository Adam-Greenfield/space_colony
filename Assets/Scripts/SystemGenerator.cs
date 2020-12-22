using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SystemGenerator
{
    #region Singleton
    public static SystemGenerator instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of system generator found");
        }
        instance = this;
    }
    #endregion
}