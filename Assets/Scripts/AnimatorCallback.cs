using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// <para>Usage: attach AnimatorCallback on the gameobject with the animator component. This class is used to expand support
/// to call any function of the form f() inside an animation event. Why? Animation events can only call functions on scripts attached to the same gameobject
/// as the animator. This means the main behavior script has to be on the same object as the animations. To not require this redesigning of prefabs and unnecessary coupling,
/// we introduce <b>AnimatorCallback</b>.</para>
/// 
/// <para> The function name for an animation event will be uniformly called "Callback" (see the callback() function in this script) and the
/// actual function that you want to call will be assigned as a string argument in the animation event. </para>
/// 
/// <para>The AnimatorCallback functions should be added during the execution of your gameobject script's Start().
/// See <see cref="AngerProjectile"/> for example. </para>
/// </summary>
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
        callbacks[targetEvent] = () => Destroy(objectToDestroy);
    }

    // this should be function called by every animation event
    public void Callback(string eventParameter)
    {
        if (callbacks.TryGetValue(eventParameter, out Action func))
        {
            func.DynamicInvoke(); // now call the corresponding callback function that the animated gameobject assigned to this event.
        }
        else
        {
            Debug.Log(gameObject.name + " does not have callback for " + eventParameter);
        }
    }
}
