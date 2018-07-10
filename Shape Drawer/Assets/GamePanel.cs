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
			case EventManager.GameEvent.EventType.NextLevel:
				myLevelData = obj.myLevelData;
				SetCountDownData();
				SetCollectableData();
				break;
			case EventManager.GameEvent.EventType.WaveFailed:
				inkSplat.color = new Color(1f, 1f, 1f, 1f);
				InkVisibleComplete();
				break;
			case EventManager.GameEvent.EventType.QuitGame:
				break;
			default:
				break;
		}
	}

	bool ShouldShowInk()
	{
		LevelType levelType = GameManager.GetCurrentLevelType();
		switch (levelType)
		{
			case LevelType.ConnecttAllMoves:
				return true;
			case LevelType.ConnectAllTimed:
				return true;
			case LevelType.ConnectAmountMoves:
				return false;
			case LevelType.ConnectAmountTime:
				return false;
			case LevelType.ConnectColor:
				break;
		}
		return true;
	}

	private void SetCollectableData()
	{
		//collectables.connectAmount = myLevelData.Goal;
		collectables.UpdateText();
	}

	void SetCountDownData()
	{
		LevelType levelType = GameManager.GetCurrentLevelType();
		levelTimer.gameObject.SetActive(false);
		movesLeft.gameObject.SetActive(false);
		switch (levelType)
		{
			case LevelType.ConnecttAllMoves:
				movesLeft.gameObject.SetActive(true);
				movesLeft.SetCountDownData(myLevelData.Moves);
				break;
			case LevelType.ConnectAllTimed:
				levelTimer.gameObject.SetActive(true);
				levelTimer.SetCountDownData(myLevelData.Time);
				break;
			case LevelType.ConnectAmountMoves:
				movesLeft.gameObject.SetActive(true);
				movesLeft.SetCountDownData(myLevelData.Moves);
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
		Hashtable table = new Hashtable();
		table.Add("from", 1);
		table.Add("to", 0);
		table.Add("time", 0.5f);
		table.Add("delay", 0.5f);
		table.Add("oncomplete", "InkInvisibleComplete");
		table.Add("oncompletetarget", gameObject);
		table.Add("onupdate", "UpdateInkColor");
		table.Add("onupdatetarget", gameObject);

		iTween.ValueTo(inkSplat.gameObject, table);

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
