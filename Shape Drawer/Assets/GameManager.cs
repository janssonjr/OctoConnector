using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelType
{
    ConnecttAll,
    Timed,
    ConnectAmount,

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
	public Transform[] transformPath;
	public Vector3[] path;

    int shapesReady;

    public static GameState myState = GameState.Length;


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
		//for(int i = 0; i < transformPath.Length; ++i)
		//{
		//	Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(transformPath[i].transform.position.x, transformPath[i].transform.position.y, 0f));
		//	path[i] = newPosition;
		//	path[i].z = 0;
		//}
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
        if(shapesReady == SpawnerManager.Instance.shapeForced.Count)
        {
            var shapedForced = SpawnerManager.Instance.shapeForced;
            for (int i = 0; i < shapedForced.Count; ++i)
            {
                if(i == 0)
                {
                    shapedForced[i].MoveToTarget();
                }
                else
                {
                    shapedForced[i].gameObject.SetActive(false);
                }
            }

        }
    }
}

[System.Serializable]
public class LevelsContainer
{
    public List<LevelData> Levels;
}