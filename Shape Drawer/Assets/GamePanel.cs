using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelData : PanelData
{
    public LevelData myLevelData;
}

public class GamePanel : Panel {

    public MovesLeft movesLeft;
    public LevelTimer levelTimer;
	public Collectables collectables;
	public Image inkSplat;
    LevelData myLevelData;

    private void Awake()
    {
        panelType = PanelEnum.InGamePanel;
    }

    private void OnEnable()
    {
		SetCollectableData();
		SetCountDownData();
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
			case EventManager.GameEvent.EventType.NewWave:
				break;
			case EventManager.GameEvent.EventType.Win:
				break;
			case EventManager.GameEvent.EventType.Lose:
				//iTween.ValueTo(inkSplat.gameObject, iTween.Hash("from", 0, "to", 1,"time", 0.01f, "oncomplete", "InkVisibleComplete", "oncompletetarget", gameObject, "onupdate", "UpdateInkColor", "onupdatetarget", gameObject));
				inkSplat.color = new Color(1f, 1f, 1f, 1f);
				InkVisibleComplete();
				break;
			case EventManager.GameEvent.EventType.ShapeReset:
				break;
			case EventManager.GameEvent.EventType.StartGame:
				break;
			case EventManager.GameEvent.EventType.PauseGame:
				break;
			case EventManager.GameEvent.EventType.ResumeGame:
				break;
			case EventManager.GameEvent.EventType.GameOver:
				break;
			case EventManager.GameEvent.EventType.LevelComplete:
				break;
			case EventManager.GameEvent.EventType.NextLevel:
				myLevelData = obj.myLevelData;
				SetCountDownData();
				SetCollectableData();
				break;
			case EventManager.GameEvent.EventType.Scored:
				break;
			case EventManager.GameEvent.EventType.QuitGame:
				break;
			default:
				break;
		}
	}

	private void SetCollectableData()
	{
		collectables.connectAmount = myLevelData.Goal;
		collectables.UpdateText();
	}

	void SetCountDownData()
	{
		LevelType levelType = GameManager.GetCurrentLevelType();
		levelTimer.gameObject.SetActive(false);
		movesLeft.gameObject.SetActive(false);
		switch (levelType)
		{
			case LevelType.ConnecttAll:
				movesLeft.gameObject.SetActive(true);
				movesLeft.SetCountDownData(myLevelData.Moves);
				break;
			case LevelType.Timed:
				levelTimer.gameObject.SetActive(true);
				levelTimer.SetCountDownData(myLevelData.Time);
				break;
			case LevelType.ConnectAmount:
				break;
			default:
				break;
		}
	}

	public void PauseGame()
    {
        EventManager.PauseGame();
        //Time.timeScale = 0f;
        CanvasManager.OpenPanel(PanelEnum.Pause);
    }

    public override void SetPanelData(PanelData aPanelData)
    {
        GamePanelData pd = aPanelData as GamePanelData;
        if(pd == null)
        {
            return;
        }
        myLevelData = pd.myLevelData;
    }

	public void InkVisibleComplete()
	{
		//iTween.ColorTo(inkSplat.gameObject, iTween.Hash("a", 0, "time", 0.5f, "delay", 0.5f, "oncomplete", "InkInvisibleComplete", "oncompletetarget", gameObject));
		iTween.ValueTo(inkSplat.gameObject, iTween.Hash("from", 1, "to", 0,"time", 0.5f, "delay", 0.5f, "oncomplete", "InkInvisibleComplete", "oncompletetarget", gameObject, "onupdate", "UpdateInkColor", "onupdatetarget", gameObject));

	}

	public void InkInvisibleComplete()
	{
		EventManager.InkDone();
	}

	public void UpdateInkColor(float alpha)
	{
		inkSplat.color = new Color(inkSplat.color.r, inkSplat.color.g, inkSplat.color.b, alpha);
	}
}
