using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovesLeft : MonoBehaviour, LevelCountDown<int>
{
    public Text myCounterText;
    //int myMovesLeft;

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
            case EventManager.GameEvent.EventType.NextLevel:
            case EventManager.GameEvent.EventType.StartGame:
			case EventManager.GameEvent.EventType.WaveComplete:
			case EventManager.GameEvent.EventType.WaveFailed:
			case EventManager.GameEvent.EventType.GameOver:
				{
					UpdateGraphics();
					break;
				}
        }
    }

    private void ReduceMoves()
    {
        UpdateGraphics();
    }

    void UpdateGraphics()
    {
        myCounterText.text = GameManager.Instance.MovesLeft.ToString();
    }

	public void SetCountDownData(int aAmount)
	{
		//myMovesLeft = aAmount;
		UpdateGraphics();
	}
}
