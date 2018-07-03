using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    Lose,
    Playing,

    Length
}

public class GameManager : MonoBehaviour
{
    public static LevelType levelType = LevelType.Length;
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
	int shapesReady;
	int shapesReadyGoal;
	int movesLeft;
	int goal;
	bool hasScoredThisWave;
    public static GameState myState = GameState.Length;
	public static GameState currentWaveState = GameState.Length;

	public int ShapeReadyGoal
	{
		set { shapesReadyGoal = value; }
		get { return shapesReadyGoal; }
	}

	private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void OnEnable()
    {
        CreateLevelData();
        EventManager.OnGameEvent += OnGameEvent;
        instance = this;
        shapesReady = 0;
		path = new Vector3[transformPath.Length];
		for (int i = 0; i < transformPath.Length; ++i)
		{
			path[i] = transformPath[i].position;
		}
		hasScoredThisWave = false;
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
                    levelType = obj.myLevelData.LevelType;
                    CurrentLevelIndex = obj.myLevelData.LevelIndex;
                    myState = GameState.Playing;
                    shapesReady = 0;
					Goal = obj.myLevelData.Goal;
					MovesLeft = obj.myLevelData.Moves;
					currentWaveState = GameState.Playing;
					hasScoredThisWave = false;
                    break;
                }
            case EventManager.GameEvent.EventType.LevelComplete:
                {
                    Levels.Levels[CurrentLevelIndex].IsComplete = true;
                    break;
                }
            case EventManager.GameEvent.EventType.NewWave:
                {
                    shapesReady = 0;
					hasScoredThisWave = false;
                    break;
                }
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
        shapesReady++;
		switch(GetCurrentLevelType())
		{
			case LevelType.ConnecttAllMoves:
			case LevelType.ConnectAllTimed:
				ShapeReadyAll();
				break;
			case LevelType.ConnectAmountMoves:
			case LevelType.ConnectAmountTime:
				ShapeReadyAmount();
				break;
		}
    }

	void ShapeReadyAll()
	{
		if (shapesReady == SpawnerManager.Instance.shapeForced.Count && hasScoredThisWave == false)
		{
			var shapedForced = SpawnerManager.Instance.shapeForced;
			ShapeReady(shapedForced);
		}
	}

	void ShapeReadyAmount()
	{
		if (shapesReady > 0 && hasScoredThisWave == false)
		{
			var shapesReady = SpawnerManager.Instance.shapeForced.FindAll(s => { return s.WasDrawn == true; });
			ShapeReady(shapesReady);	
		}
	}

	void ShapeReady(List<Shape> aShapeList)
	{
		for (int i = 0; i < aShapeList.Count; ++i)
		{
			if (i == 0)
			{
				aShapeList[i].MoveToTarget();
				hasScoredThisWave = true;
			}
			else
			{
				aShapeList[i].gameObject.SetActive(false);
			}
		}
	}

	public void ShapeDrawn()
	{
		CheckForWinner();
	}

	void CheckForWinner()
	{
		if (currentWaveState == GameState.Playing)
		{
			switch (GetCurrentLevelType())
			{
				case LevelType.ConnecttAllMoves:
					DrawnAll();
					break;
				case LevelType.ConnectAllTimed:
					DrawnAll();
					break;
				case LevelType.ConnectAmountMoves:
					CountDrawnShapes();
					break;
			}
		}
	}

	void CountDrawnShapes()
	{
		SpawnerManager spawner = SpawnerManager.Instance;
		int drawCount = spawner.shapeForced.Count(s => { return s.WasDrawn == true; });
		if (drawCount > 0 && drawCount == shapesReadyGoal)
		{
			currentWaveState = GameState.Won;
			spawner.shapeForced.ForEach(s => { if(s.WasDrawn) s.MoveToCenter(); });
			Goal -= drawCount;
			MovesLeft--;
			EventManager.WaveComplete();
		}
	}

	void DrawnAll()
	{
		SpawnerManager spawner = SpawnerManager.Instance;
		int drawCount = spawner.shapeForced.Count(s => { return s.WasDrawn == true; });
		if (drawCount == spawner.shapeForced.Count)
		{
			spawner.shapeForced.ForEach(s => { s.MoveToCenter(); });
			WaveComplete();
			//EventManager.Win(GetCurrentLevelType());
		}
	}

	public void WaveFailed()
	{
		MovesLeft--;
		currentWaveState = GameState.Lose;
		if(MovesLeft <= 0 && myState != GameState.Won)
		{
			EventManager.GameOver();
			CanvasManager.OpenPanel(PanelEnum.GameOverPanel);
		}
		else
		{
			EventManager.WaveFailed();
		}
	}

	public void WaveComplete()
	{
		currentWaveState = GameState.Playing;
		MovesLeft--;
		Goal--;
		EventManager.WaveComplete();
		
	}

	public void CheckLevelComplete()
	{
		if (Goal <= 0)
		{
			myState = GameState.Won;
			EventManager.LevelComplete();
			CanvasManager.OpenPanel(PanelEnum.LevelComplete);
		}
	}
}

[System.Serializable]
public class LevelsContainer
{
    public List<LevelData> Levels;
}