using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public enum LevelType
{
    ConnecttAllMoves,
    ConnectAllTimed,
    ConnectAmountMoves,
	ConnectAmountTime,
	ConnectColor,

    Length
}

public enum GameState
{
    Won,
    Lost,
    Playing,

    Length
}

public class GameManager : MonoBehaviour
{
    public static int CurrentLevelIndex = 0;
    public static LevelsContainer Levels;
	public int Goal
	{
		get
		{
			return goal;
		}
		private set
		{
			goal = value;
			if(goal <= 0)
			{
				goal = 0;
			}
		}
	}
	public int MovesLeft
	{
		get
		{
			return movesLeft;
		}
		private set
		{
			movesLeft = value;
			if(movesLeft <= 0)
			{
				movesLeft = 0;
			}
		}
	}

	public Transform[] transformPath;
	public Vector3[] path;
	public float delay = 0.3f;
	public float tweenTime = 0.9f;
	int movesLeft;
	int goal;

	public static GameState myWaveState = GameState.Length;
    public static GameState myLevelState = GameState.Length;

	public BaseGameModeSettings Settings { get; private set; }


	private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

	private void Awake()
	{
		DOTween.Init();
	}

	private void OnEnable()
    {
        CreateLevelData();
        EventManager.OnGameEvent += OnGameEvent;
        instance = this;
		path = new Vector3[transformPath.Length];
		for (int i = 0; i < transformPath.Length; ++i)
		{
			path[i] = transformPath[i].position;
		}
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
                    CurrentLevelIndex = obj.myLevelData.LevelIndex;
                    myLevelState = GameState.Playing;
					Goal = obj.myLevelData.Goal;
					MovesLeft = obj.myLevelData.Moves;
					InitializeGameSettings();
					Settings.Reset();
                    break;
                }
            case EventManager.GameEvent.EventType.LevelComplete:
                {
                    Levels.Levels[CurrentLevelIndex].IsComplete = true;
                    break;
                }
            case EventManager.GameEvent.EventType.NewWave:
                {
					Settings.Reset();
					myWaveState = GameState.Playing;
                    break;
                }
			case EventManager.GameEvent.EventType.PreScore:
				myWaveState = GameState.Won;
				break;
        }
    }

	private void InitializeGameSettings()
	{
		switch (GetCurrentLevelType())
		{
			case LevelType.ConnecttAllMoves:
				Settings = new ConnectAllGameMode();
				break;
			case LevelType.ConnectAllTimed:
				Settings = new ConnectAllTimedGameMode();
				break;
			case LevelType.ConnectAmountMoves:
				Settings = new ConnectAmountGameMode();
				break;
			case LevelType.ConnectAmountTime:
				break;
			case LevelType.ConnectColor:
				break;
			default:
				break;
		}
	}

	public static bool IsLastLevel()
    {
        return CurrentLevelIndex == (Levels.Levels.Count - 1);
    }

    void CreateLevelData()
    {
        TextAsset levelText = Resources.Load("Levels") as TextAsset;
        Levels = JsonUtility.FromJson<LevelsContainer>(levelText.text);
    }

    public static LevelData GetNextLevel()
    {
        int nextLevelIndex = CurrentLevelIndex + 1;
        return Levels.Levels.Find(data => 
        {
            return data.LevelIndex == nextLevelIndex;
        });
    }

    public static LevelData GetCurrentLevelData()
    {
        return Levels.Levels[CurrentLevelIndex];
    }


    public static LevelType GetCurrentLevelType()
    {
        return Levels.Levels[CurrentLevelIndex].LevelType;
    }

    public void ShapeReady()
    {
		myWaveState = GameState.Won;
		Settings.ShapesReadyToFly();
    }

	//void ShapeReadyAmount()
	//{
	//	if (shapesReady > 0 && hasScoredThisWave == false)
	//	{
	//		var shapesReady = SpawnerManager.Instance.shapeForced.FindAll(s => { return s.WasDrawn == true; });
	//		ShapeReady(shapesReady);	
	//	}
	//}



	public void CheckIfCanScore()
	{
		Settings.CheckIfCanScore();
	}

	void CountDrawnShapes()
	{
		//SpawnerManager spawner = SpawnerManager.Instance;
		//int drawCount = spawner.shapeForced.Count(s => { return s.WasDrawn == true; });
		//if (drawCount > 0 && drawCount == shapesReadyGoal)
		//{
		//	currentWaveState = GameState.Won;
		//	spawner.shapeForced.ForEach(s => { if(s.WasDrawn) s.MoveToCenter(); });
		//	Goal -= drawCount;
		//	MovesLeft--;
		//	EventManager.WaveComplete();
		//}
	}

	public void WaveFailed()
	{
		MovesLeft--;
		myWaveState = GameState.Lost;
		if(MovesLeft <= 0 && myLevelState != GameState.Won)
		{
			EventManager.GameOver();
			CanvasManager.OpenPanel(PanelEnum.GameOverPanel);
		}
		else if(myWaveState != GameState.Won)
		{
			EventManager.WaveFailed();
		}
	}

	public void WaveComplete()
	{
		MovesLeft--;
		Goal -= Settings.GoalAmout;		
	}

	public void CheckLevelComplete()
	{
		WaveComplete();
		if (Goal <= 0)
		{
			myLevelState = GameState.Won;
			EventManager.LevelComplete();
			CanvasManager.OpenPanel(PanelEnum.LevelComplete);
		}
		else
		{
			EventManager.WaveComplete();
		}
	}
}

[System.Serializable]
public class LevelsContainer
{
    public List<LevelData> Levels;
}