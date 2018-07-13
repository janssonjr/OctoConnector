using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class SpawnerManager : MonoBehaviour {

    public List<Rigidbody2D> shapes = new List<Rigidbody2D>();
    public List<Shape> shapeForced = new List<Shape>();
	public BoxCollider2D box;
    private static SpawnerManager instance;

    public static SpawnerManager Instance
    {
        get { return instance; }
    }

    int collisionCount = 0;

    bool shouldCheckWinCondition = false;

    bool isRunning = false;

    private void OnEnable()
    {
        instance = this;
        EventManager.OnGameEvent += OnGameState;
    }

    private void OnDisable()
    {
        EventManager.OnGameEvent -= OnGameState;
    }

    private void OnGameState(EventManager.GameEvent obj)
    {
        switch (obj.myType)
        {
            case EventManager.GameEvent.EventType.NewWave:
                break;
            //case EventManager.GameEvent.EventType.Win:
            //    {
            //        //StartCoroutine(ResetShapesRoutine(1f));
            //        shouldCheckWinCondition = false;
            //    }
                //break;
            case EventManager.GameEvent.EventType.StartGame:
                {
                    Debug.Log("Starting Game");
                    StopAllCoroutines();
                    isRunning = true;
                    shouldCheckWinCondition = false;
                    collisionCount = 0;
                    ResetAllShapes();
                    StartCoroutine(AddForce());

                }
                break;
            case EventManager.GameEvent.EventType.GameOver:
            case EventManager.GameEvent.EventType.LevelComplete:
            case EventManager.GameEvent.EventType.PauseGame:
                isRunning = false;
                break;
            case EventManager.GameEvent.EventType.ResumeGame:
                isRunning = true;
                break;
            case EventManager.GameEvent.EventType.NextLevel:
                {

                    ResetAllShapes();
                    StopAllCoroutines();
                    isRunning = true;
                    collisionCount = 4;
                    StartCoroutine(AddForce());

                    break;
                }
            case EventManager.GameEvent.EventType.ShapeReset:
                {
                    SetShapesActive();
                    ResetShapesPosition();
                    StartCoroutine(ResetShapesRoutine(1f));
                    break;
                }
			case EventManager.GameEvent.EventType.InkDone:
					isRunning = true;
				break;
        }
    }

    private void SetShapesActive()
    {
        for(int i = 0; i < shapes.Count; ++i)
        {
            shapes[i].gameObject.SetActive(true);
        }
    }

    IEnumerator AddForce()
    {
        yield return new WaitForSeconds(2);
        while (true)
        {
            yield return new WaitForSeconds(1f);
			int range = UnityEngine.Random.Range(2, shapes.Count + 1);
			List<Vector2> colliders = new List<Vector2>();
			if (GameManager.myLevelState == GameState.Playing)
			{
				shapeForced.Clear();
				EventManager.NewWave();
				shouldCheckWinCondition = true;
				Shuffle();
				for (int i = 0; i < range; ++i)
				{
					shapes[i].transform.localPosition = GetRandomPositionInCollider();
					bool iscollidingWithOther = true;
					while(iscollidingWithOther)
					{
						bool isNotTouchingAny = colliders.TrueForAll(b =>
						{
							return Mathf.Abs(b.x - shapes[i].transform.localPosition.x) > 0.7;
						});

						if (isNotTouchingAny == true)
							break;
						shapes[i].transform.localPosition = GetRandomPositionInCollider();

					}

					shapes[i].AddForce(Vector2.up * UnityEngine.Random.Range(520, 700));
					shapeForced.Add(shapes[i].GetComponent<Shape>());
					shapeForced[shapeForced.Count - 1].Launched();

					yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.3f));
					colliders.Add(shapes[i].transform.localPosition);

				}
			}
            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() =>
            {
                return collisionCount == shapes.Count && isRunning == true;
            });
        }
    }

    void Shuffle()
    {
        for(int i =0; i < shapes.Count; ++i)
        {
            var temp = shapes[i];
            int randomIndex = UnityEngine.Random.Range(i, shapes.Count);
            shapes[i] = shapes[randomIndex];
            shapes[randomIndex] = temp;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Shape"))
        {
            collisionCount++;
            Debug.Log("OnEnter: CollisionCount: " + collisionCount.ToString());
            Shape shapeToRemove = collision.gameObject.GetComponent<Shape>();
            if (shapeForced.Find(a => { return a == shapeToRemove; }) == null)
                return;
            if (shapeToRemove.WasDrawn == false && shapeForced.Count > 0
				&& shouldCheckWinCondition == true && GameManager.myWaveState != GameState.Won)
            {
				GameManager.Instance.WaveFailed();
                shouldCheckWinCondition = false;
				isRunning = false;
                StartCoroutine(ResetShapesRoutine(1f));
            }            
            shapeToRemove.OnGround();
        }
    }

    IEnumerator ResetShapesRoutine(float aWaitTime)
    {
        yield return new WaitForSeconds(aWaitTime);
        ResetShapes();
    }

    void ResetShapes()
    {
        foreach (var shape in shapes)
        {
            shape.bodyType = RigidbodyType2D.Dynamic;
			Debug.Log("SpawnerManager.ResetShapes: Changing "+shape.gameObject.name+" to dynamic");

		}
	}

    public void ResetAllShapes()
    {
		RemoveShapeTween();
        ResetShapesPosition();
        ResetShapes();
        SetShapesActive();
		ResetShapesRotation();
		ResetShapeScale();
    }

	private void ResetShapeScale()
	{
		foreach (var s in shapes)
		{
			s.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	void ResetShapesRotation()
	{
		foreach (var s in shapes)
		{
			s.transform.localRotation = Quaternion.identity;
		}

	}

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Shape"))
        {
            if(collisionCount > 0)
                collisionCount--;
            Debug.Log("OnExits: CollisionCount: " + collisionCount.ToString());
        }
    }

    void ResetShapesPosition()
    {
        foreach (var s in shapes)
        {
            Shape shape = s.GetComponent<Shape>();
            shape.ResetPosition();
        }
    }

	void RemoveShapeTween()
	{
		foreach (var s in shapes)
		{
			Shape shape = s.GetComponent<Shape>();
			shape.RemoveTween();
		}
	}

	Vector2 GetRandomPositionInCollider()
	{
		Vector2 returnVector = Vector2.zero;
		float cameraSize = Camera.main.orthographicSize;
		returnVector.x = UnityEngine.Random.Range(-((cameraSize - 1) / 2), (cameraSize - 1) / 2);
		returnVector.y = -6f;

		return returnVector;
	}
}
