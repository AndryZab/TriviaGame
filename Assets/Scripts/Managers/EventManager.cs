using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static event Action OnVictory;
    public static event Action OnLosing;

    public static void EventOnVictory()
    {
        SafeTriggerEvent(OnVictory);
    }

    public static void EventOnLosing()
    {
        SafeTriggerEvent(OnLosing);
    }

    private static void SafeTriggerEvent(Action eventAction)
    {
        if (eventAction == null)
            return;

        var exceptions = new List<Exception>();

        foreach (var handler in eventAction.GetInvocationList())
        {
            try
            {
                ((Action)handler).Invoke();
            }
            catch (Exception e)
            {
                exceptions.Add(e);
                Debug.LogException(e);
            }
        }

        if (exceptions.Count > 0)
        {
            Debug.LogWarning($"Event triggered with {exceptions.Count} handler exceptions.");
        }
    }
}
