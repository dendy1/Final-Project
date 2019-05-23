using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, EventHandler<EventArgs>> _eventsDictionary;

    private static EventManager _instance;

    public static EventManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(EventManager)) as EventManager;
                
                if (!_instance)
                {
                    Debug.LogError("Please, use one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    _instance.Initialize();
                }
            }

            return _instance;
        }
    }

    private void Initialize()
    {
        if (_eventsDictionary == null)
            _eventsDictionary = new Dictionary<string, EventHandler<EventArgs>>(    );
    }

    public void AddListener(string eventName, EventHandler<EventArgs> callback)
    {
        EventHandler<EventArgs> eventCallbacks;

        if (_eventsDictionary.TryGetValue(eventName, out eventCallbacks))
        {
            eventCallbacks += callback;
            _eventsDictionary[eventName] = eventCallbacks;
        }
        else
        {
            eventCallbacks += callback;
            _eventsDictionary.Add(eventName, eventCallbacks);
        }
    }

    public void RemoveListener(string eventName, EventHandler<EventArgs> callback)
    {
        EventHandler<EventArgs> eventCallbacks;
        
        if (_eventsDictionary.TryGetValue(eventName, out eventCallbacks))
        {
            eventCallbacks -= callback;
            _eventsDictionary[eventName] = eventCallbacks;
        }
    }

    public void Invoke(string eventName, object sender, EventArgs args)
    {
        EventHandler<EventArgs> eventCallbacks;

        if (Instance._eventsDictionary.TryGetValue(eventName, out eventCallbacks))
        {
            eventCallbacks.Invoke(sender, args);
        }
    }
}
