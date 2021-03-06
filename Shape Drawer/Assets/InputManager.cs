﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour {

    public SpawnerManager spawner;
    bool shouldCheckForInput;
    public CircleCollider2D circleCollider;
    public LineRenderer line;
    List<Vector3> taggedShapes = new List<Vector3>();
    List<LineRenderer> lines = new List<LineRenderer>();

    private void OnEnable()
    {
        shouldCheckForInput = false;
        circleCollider.enabled = false;
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
            case EventManager.GameEvent.EventType.StartGame:
                shouldCheckForInput = true;
                circleCollider.enabled = false;
                break;
            case EventManager.GameEvent.EventType.NewWave:
                {
                    shouldCheckForInput = true;
                    circleCollider.enabled = false;
                    taggedShapes.Clear();
                    break;
                }
            case EventManager.GameEvent.EventType.ShapeReset:
                {
                    line.positionCount = 0;
                    ResetLines();
                    break;
                }
			case EventManager.GameEvent.EventType.WaveComplete:
			case EventManager.GameEvent.EventType.PreScore:
            case EventManager.GameEvent.EventType.GameOver:
            case EventManager.GameEvent.EventType.LevelComplete:
				shouldCheckForInput = false;
				ResetLines();
				Debug.Log("WaveComplete, GameOver or LevelComplete");
				break;
            case EventManager.GameEvent.EventType.PauseGame:
                {
					shouldCheckForInput = false;
                    break;
                }
            case EventManager.GameEvent.EventType.ResumeGame:
                {
					shouldCheckForInput = true;
                    break;
                }
            case EventManager.GameEvent.EventType.NextLevel:
                {
                    taggedShapes.Clear();
                    shouldCheckForInput = true;
                    break;
                }
        }
    }

    void ResetLines()
    {
        for (int i = 0; i < lines.Count; ++i)
        {
            Destroy(lines[i].gameObject);
        }
        lines.Clear();
		Debug.Log("Reseting lines: " + lines.Count);
		
    }

    void Update ()
    {
        if (shouldCheckForInput == false)
            return;

        if (Input.GetMouseButton(0))
        {
            if(shouldCheckForInput == true)
            {
                circleCollider.enabled = true;
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 origin = new Vector2(worldPoint.x, worldPoint.y);
                transform.position = origin;
                if (lines.Count > 0)
                {
                    lines[lines.Count - 1].positionCount = 2;
                    worldPoint.z = 0;
                    lines[lines.Count - 1].SetPosition(1, worldPoint);
                }
            }
            else if(shouldCheckForInput == false && lines.Count > taggedShapes.Count)
            {
                lines[lines.Count - 1].SetPosition(1, taggedShapes[taggedShapes.Count - 1]);
            }
            
            
        }
        else if(Input.GetMouseButtonUp(0))
        {
            circleCollider.enabled = false;
            if (shouldCheckForInput == true)
            {
				GameManager.Instance.Settings.MouseUpAction();                
            }
            ResetLines();
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shouldCheckForInput == false)
            return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Shape"))
        {
            Shape shape = collision.gameObject.GetComponentInParent<Shape>();
            SpriteRenderer sprite = shape.spriteRenderer;
            if(shape.WasDrawn == false && sprite.isVisible == true)
            {
                shape.OnPressed();
                shape.rb.bodyType = RigidbodyType2D.Static;
				Debug.Log("Changing "+shape.name+" to static");
                taggedShapes.Add(collision.transform.position);
                if(lines.Count > 0)
                {
                    lines[lines.Count - 1].SetPosition(1, new Vector3(shape.transform.position.x, shape.transform.position.y, 0));
                }

                if (taggedShapes.Count < spawner.shapeForced.Count)
                {
                    lines.Add(Instantiate(line, transform));

                    lines[lines.Count - 1].positionCount = 1;
                    lines[lines.Count - 1].SetPosition(0, new Vector3(shape.transform.position.x, shape.transform.position.y, 0));
                }
				LevelType levelType = GameManager.GetCurrentLevelType();
				if (levelType == LevelType.ConnecttAllMoves || levelType == LevelType.ConnectAllTimed)
				{
					GameManager.Instance.CheckIfCanScore();
				}
				else
				{
					int count = spawner.shapeForced.Count(s => { return s.WasDrawn == true; });
					if (count == spawner.shapeForced.Count)
					{
						//GameManager.Instance.ShapeReadyGoal = count;
						GameManager.Instance.CheckIfCanScore();
					}
				}
			}
        }
    }
}

