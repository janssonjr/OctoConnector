using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectables : MonoBehaviour {

    public Text amountText;
    //public int connectAmount;
    public Image img;

    private void OnEnable()
    {
        EventManager.OnGameEvent += OnGameEvent;
        //connectAmount = 5;
    }

    private void OnDisable()
    {
        EventManager.OnGameEvent -= OnGameEvent;
    }

    private void OnGameEvent(EventManager.GameEvent obj)
    {
        switch (obj.myType)
        {
            //case EventManager.GameEvent.EventType.Scored:
            //    AddScore(GameManager.levelType);
                //break;
            case EventManager.GameEvent.EventType.NextLevel:
            case EventManager.GameEvent.EventType.StartGame:
                //connectAmount = obj.myLevelData.Goal;
                UpdateText();
                break;
            default:
                break;
        }
    }

    private void AddScore(LevelType aLevelType)
    {
        switch (aLevelType)
        {
            case LevelType.ConnecttAll:
                //ReduceConnectGoal();
                UpdateText();
                break;
            case LevelType.Timed:
                //ReduceConnectGoal();
                UpdateText();
                break;
            case LevelType.ConnectAmount:
                break;
            default:
                break;
        }
    }

    //private void ReduceConnectGoal()
    //{
    //    connectAmount--;
    //    if (connectAmount < 0)
    //        connectAmount = 0;
    //    if(connectAmount == 0)
    //    {
    //        GameManager.myState = GameState.Won;
    //        EventManager.LevelComplete();
    //        CanvasManager.OpenPanel(PanelEnum.LevelComplete);
    //    }
    //}

    public void UpdateText()
    {
        amountText.text = GameManager.Instance.Goal.ToString();
    }
}
