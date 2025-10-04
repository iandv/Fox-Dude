using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance
    {
        get; private set;
    }

    private Dictionary<string, Action<object[]>> _subscribers = new Dictionary<string, Action<object[]>>();

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning($"Duplicated EventManager found in GameObject: {gameObject.name}");
            Destroy(this);
        }
    }

    public void Subscribe(string eventID, Action<object[]> callback)
    {
        if (!_subscribers.ContainsKey(eventID))
            _subscribers.Add(eventID, callback);
        else
            _subscribers[eventID] += callback;
    }

    public void Unsubscribe(string eventID, Action<object[]> callback)
    {
        if (!_subscribers.ContainsKey(eventID)) return;

        _subscribers[eventID] -= callback;
    }

    public void Trigger(string eventID, params object[] parameters)
    {
        if (!_subscribers.ContainsKey(eventID)) return;

        _subscribers[eventID]?.Invoke(parameters);
    }
}
