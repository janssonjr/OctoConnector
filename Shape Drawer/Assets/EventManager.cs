using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour {

    public static Action<GameEvent> OnGameEvent;

    public class GameEvent
    {
        public enum EventType
        {
            NewWave,
            Win, 
            Lose,
            ShapeReset,
            StartGame,
            PauseGame,
            ResumeGame,
            GameOver,
            LevelComplete,
			NextLevel,
            Scored,
            QuitGame,
			InkDone
		}
        public LevelType myLevelType;
        public EventType myType;
        public LevelData myLevelData;
    }

    public static void NewWave()
    {
        if (OnGameEvent != null)
            OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.NewWave});
    }

    public static void Win(LevelType aLevelType)
    {
        if (OnGameEvent != null)
            OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.Win, myLevelType = aLevelType });
    }

    public static void Lose()
    {
        if (OnGameEvent != null)
            OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.Lose });
    }

    public static void ShapeReset()
    {
        if (OnGameEvent != null)
            OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.ShapeReset });
    }

    public static void StartGame(LevelData aLevelData)
    {
        if (OnGameEvent != null)
            OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.StartGame, myLevelData = aLevelData });
    }

    public static void PauseGame()
    {
        if (OnGameEvent != null)
            OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.PauseGame });
    }

    public static void ResumeGame()
    {
        if (OnGameEvent != null)
            OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.ResumeGame });
    }

    public static void GameOver()
    {
        if (OnGameEvent != null)
            OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.GameOver });
    }

    public static void LevelComplete()
    {
        if (OnGameEvent != null)
            OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.LevelComplete });
    }

    public static void NextLevel(LevelData aLevelData)
    {
        if (OnGameEvent != null)
            OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.NextLevel, myLevelData = aLevelData });
    }

    public static void Scored()
    {
        if (OnGameEvent != null)
            OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.Scored});
    }

	public static void InkDone()
	{
		if (OnGameEvent != null)
			OnGameEvent.Invoke(new GameEvent { myType = GameEvent.EventType.InkDone });
	}
}