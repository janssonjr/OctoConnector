using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectables : MonoBehaviour {

    public Text amountText;
    public Image img;

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
        switch (obj.myType)
        {
            case EventManager.GameEvent.EventType.NextLevel:
            case EventManager.GameEvent.EventType.StartGame:
			case EventManager.GameEvent.EventType.WaveComplete:
			case EventManager.GameEvent.EventType.LevelComplete:
                UpdateText();
                break;
            default:
                break;
        }
    }

    public void UpdateText()
    {
        amountText.text = GameManager.Instance.Goal.ToString();
    }
}
