using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScoreType
{
    Win,
    Lose
}

public class Score : MonoBehaviour {

    public Text myText;
    public ScoreType myScoreType;
    int myScore;

    private void OnEnable()
    {
        myScore = 0;
        if(myScoreType == ScoreType.Win)
            UpdateText("Wins: ");
        else
            UpdateText("Loses: ");
        EventManager.OnGameEvent += OnGameEvent;
    }

    private void OnDisable()
    {
        EventManager.OnGameEvent -= OnGameEvent;
    }

    private void OnGameEvent(EventManager.GameEvent obj)
    {
        //if(obj.myType == EventManager.GameEvent.EventType.Win && myScoreType == ScoreType.Win)
        //{
        //    myScore++;
        //    UpdateText("Wins: ");
        //}
        //else if(obj.myType == EventManager.GameEvent.EventType.Lose && myScoreType == ScoreType.Lose)
        //{
        //    myScore++;
        //    UpdateText("Loses: ");
        //}
    }

    void UpdateText(string aText)
    {
        myText.text = aText + myScore.ToString();
    }
}
