using UnityEngine;
using System.Collections;

public interface ICameraTarget : IMonoBehaviour
{
    int distanceFromCore
    {
        get;
        set;
    }
}

public interface IMonoBehaviour
{
    Transform transform { get; }
    GameObject gameObject { get; }
    T GetComponent<T>();
}

