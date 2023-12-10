using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorCallback : MonoBehaviour
{
    private Dictionary<string, Action> callbacks;

    void Awake()
    {
        callbacks = new Dictionary<string, Action>();
    }

    /// <summary>
    /// assigns the callback function f to be triggered when animation event fires with value targetEvent
    /// </summary>
    /// <param name="targetEvent">event argument</param>
    /// <param name="f">function/procedure (method with 0 arguments and returns void)</param>
    public void AddCallback(string targetEvent, Action f)
    {
        callbacks[targetEvent] = f;
    }

    /// <summary>
    /// creates callback to destroy the given gameobject when animation event is fired.
    /// </summary>
    /// <param name="targetEvent">event argument</param>
    /// <param name="objectToDestroy">reference to the gameobject that should be deleted</param>
    public void AddDestroyCallback(string targetEvent, GameObject objectToDestroy)
    {
        print(targetEvent);
        print(callbacks);
        callbacks[targetEvent] = () => Destroy(objectToDestroy);
    }

    // this should be function called by every animation event
    public void Callback(string eventParameter)
    {
        if (callbacks.TryGetValue(eventParameter, out Action func))
        {
            func.DynamicInvoke();
        }
        else
        {
            Debug.Log(gameObject.name + " does not have callback for " + eventParameter);
        }
    }
}
