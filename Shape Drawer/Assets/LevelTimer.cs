using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour, LevelCountDown<float>
{
	public Text myText;
    float myTimer;
	bool haveLost;
	bool shouldCount;

	private void OnEnable()
	{
		shouldCount = true;
		EventManager.OnGameEvent += OnGameEvent;
	}

	private void OnDisable()
	{
		EventManager.OnGameEvent -= OnGameEvent;
	}

	private void OnGameEvent(EventManager.GameEvent obj)
	{
		if (GameManager.GetCurrentLevelType() == LevelType.Timed)
		{

			switch (obj.myType)
			{
				case EventManager.GameEvent.EventType.StartGame:
					shouldCount = true;
					break;
				case EventManager.GameEvent.EventType.PauseGame:
					shouldCount = false;
					break;
				case EventManager.GameEvent.EventType.ResumeGame:
					shouldCount = true;
					break;
				case EventManager.GameEvent.EventType.GameOver:
					shouldCount = false;
					break;
				case EventManager.GameEvent.EventType.LevelComplete:
					shouldCount = false;
					break;
				default:
					break;
			}
		}
	}

	public void SetCountDownData(float aAmount)
    {
        myTimer = aAmount;
		haveLost = false;
    }

	void Update ()
    {
		if (shouldCount == false)
			return;
		myTimer -= Time.deltaTime;
		if(myTimer <= 0)
		{
			if(haveLost == false)
			{
				EventManager.GameOver();
				haveLost = true;
				CanvasManager.OpenPanel(PanelEnum.GameOverPanel);
			}
			myTimer = 0f;
		}

		UpdateGraphics();
	}

	void UpdateGraphics()
	{
		myText.text = ((int)myTimer).ToString();
	}
}
