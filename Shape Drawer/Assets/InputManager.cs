using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public SpawnerManager spawner;
    //public ParticleSystem particles;
    bool shouldCheckForWinner;
    public CircleCollider2D circleCollider;
    public LineRenderer line;
    List<Vector3> taggedShapes = new List<Vector3>();
    List<LineRenderer> lines = new List<LineRenderer>();
    bool isPaused;
    private void OnEnable()
    {
        shouldCheckForWinner = true;
        circleCollider.enabled = false;
        EventManager.OnGameEvent += OnGameEvent;
        //line.positionCount = 0;
        isPaused = false;
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
                shouldCheckForWinner = true;
                circleCollider.enabled = false;
                isPaused = false;
                break;
            case EventManager.GameEvent.EventType.NewWave:
                {
                    shouldCheckForWinner = true;
                    circleCollider.enabled = false;
                    taggedShapes.Clear();
                    break;
                }
            case EventManager.GameEvent.EventType.Lose:
                {
                    shouldCheckForWinner = false;
                    circleCollider.enabled = false;
                    line.positionCount = 0;
                    ResetLines();
                    break;
                }
            case EventManager.GameEvent.EventType.ShapeReset:
                {
                    line.positionCount = 0;
                    ResetLines();
                    break;
                }
            case EventManager.GameEvent.EventType.GameOver:
            case EventManager.GameEvent.EventType.LevelComplete:
            case EventManager.GameEvent.EventType.PauseGame:
                {
                    isPaused = true;
                    break;
                }
            case EventManager.GameEvent.EventType.ResumeGame:
                {
                    isPaused = false;
                    break;
                }
            case EventManager.GameEvent.EventType.NextLevel:
                {
                    isPaused = false;
                    taggedShapes.Clear();
                    shouldCheckForWinner = true;
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
    }

    void Update ()
    {
        if (isPaused == true)
            return;
        if (shouldCheckForWinner == false)
            return;
        int drawCount = 0;
        if (spawner.shapeForced.Count > 0)
        {
            for (int i = 0; i < spawner.shapeForced.Count; ++i)
            {
                if (spawner.shapeForced[i].WasDrawn == true)
                    drawCount++;
            }
            if (drawCount == spawner.shapeForced.Count)
            {
                shouldCheckForWinner = false;
                
                EventManager.Win(GameManager.GetCurrentLevelType());
                line.positionCount = 0;
                ResetLines();
                //ResetLines();
            }
        }
        if (Input.GetMouseButton(0))
        {
            //particles.gameObject.SetActive(true);
            if(shouldCheckForWinner == true)
            {
                circleCollider.enabled = true;
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //particles.transform.position = worldPoint;
                Vector2 origin = new Vector2(worldPoint.x, worldPoint.y);
                transform.position = origin;
                if (lines.Count > 0)
                {
                    lines[lines.Count - 1].positionCount = 2;
                    worldPoint.z = 0;
                    lines[lines.Count - 1].SetPosition(1, worldPoint);
                }
            }
            else if(shouldCheckForWinner == false && lines.Count > taggedShapes.Count)
            {
                lines[lines.Count - 1].SetPosition(1, taggedShapes[taggedShapes.Count - 1]);
            }
            
            
        }
        else if(Input.GetMouseButtonUp(0))
        {
            //particles.gameObject.SetActive(false);
            circleCollider.enabled = false;
            if (shouldCheckForWinner == true)
            {
                if (spawner.shapeForced.Count > 0)
                {
                    foreach (var shape in spawner.shapeForced)
                    {
                        shape.WasDrawn = false;
                        shape.canBePressed = true;
                        //shape.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f);
                        shape.rb.bodyType = RigidbodyType2D.Dynamic;
                    }
                }
            }
            ResetLines();
        }

        
        
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPaused == true)
            return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Shape"))
        {
            Shape shape = collision.gameObject.GetComponentInParent<Shape>();
            SpriteRenderer sprite = shape.spriteRenderer;
            if(shape.WasDrawn == false && sprite.isVisible == true)
            {
                shape.OnPressed();
                shape.rb.bodyType = RigidbodyType2D.Static;
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
            }
            //collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f);
            //line.positionCount = taggedShapes.Count;
            //line.SetPosition(taggedShapes.Count - 1, collision.transform.position);
        }
    }
}
