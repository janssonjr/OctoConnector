using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{

    private void OnEnable()
    {
        EventManager.OnGameEvent += OnGameEvent;
    }

    private void OnDisable()
    {
        EventManager.OnGameEvent -= OnGameEvent;
    }

    private void OnGameEvent(EventManager.GameEvent obj)
    {
        switch(obj.myType)
        {
            case EventManager.GameEvent.EventType.Win:
                Camera.main.backgroundColor = new Color(0f, 1f, 0f, 1f);
                break;
            case EventManager.GameEvent.EventType.Lose:
                Camera.main.backgroundColor = new Color(1f, 0f, 0f, 1f);
                break;
            case EventManager.GameEvent.EventType.NewWave:
                Camera.main.backgroundColor = new Color(1f, 1f, 1f, 1f);
                break;
            case EventManager.GameEvent.EventType.NextLevel:
            case EventManager.GameEvent.EventType.StartGame:
                Camera.main.backgroundColor = new Color(1f, 1f, 1f, 1f);
                break;
        }
    }
}
