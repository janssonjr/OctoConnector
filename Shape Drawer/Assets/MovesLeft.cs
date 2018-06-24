using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovesLeft : MonoBehaviour, LevelCountDown<int>
{
    public Text myCounterText;
    int myMovesLeft;

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
                {
                    myMovesLeft = obj.myLevelData.Moves;
                    UpdateGraphics();
                break;
                }
            case EventManager.GameEvent.EventType.Win:
                {
                    ReduceMoves();
                    break;
                }
            case EventManager.GameEvent.EventType.Lose:
                {
                    ReduceMoves();
                    break;
                }
        }
    }

    private void ReduceMoves()
    {
        myMovesLeft--;
        if (myMovesLeft < 0)
        {
            myMovesLeft = 0;
        }
        if(myMovesLeft == 0 && GameManager.myState != GameState.Won)
        {
            EventManager.GameOver();
            CanvasManager.OpenPanel(PanelEnum.GameOverPanel);
        }
        UpdateGraphics();
    }

    void UpdateGraphics()
    {
        myCounterText.text = myMovesLeft.ToString();
    }

	public void SetCountDownData(int aAmount)
	{
		myMovesLeft = aAmount;
		UpdateGraphics();
	}
}
