using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpriteStage
{
    Up,
    Down,
    Connecting
}

public class Shape : MonoBehaviour {

	public Animator myConnectingAnimator;
	public Animator myFlyAnimator;
    public List<Sprite> sprites = new List<Sprite>();
    public bool WasDrawn = false;
    public bool canBePressed = false;
    public SpriteStage myStage;
    public GameObject leftArm;
    public GameObject rightArm;

    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;

	public ParticleSystem scoreParticles;

    Vector2 pauseVelocity;
    RectTransform collectable;
    Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        //rb = GetComponentInChildren<Rigidbody2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        EventManager.OnGameEvent += onGameEvent;
		myConnectingAnimator.enabled = false;
		myFlyAnimator.enabled = false;

	}

    private void onGameEvent(EventManager.GameEvent obj)
    {
        if(obj.myType == EventManager.GameEvent.EventType.Win)
        {
            if (WasDrawn == true)
            {
                iTween.MoveTo(rb.gameObject, iTween.Hash(
                    "position", GameManager.Instance.transformPath[0].position,
                    "time", 1f,
                    "oncompletetarget",  GameManager.Instance.gameObject,
                    "oncomplete", "ShapeReady"
                    ));
            }
        }
        if(obj.myType == EventManager.GameEvent.EventType.PauseGame)
        {
            pauseVelocity = rb.velocity;
            rb.bodyType = RigidbodyType2D.Static;
        }
        else if(obj.myType == EventManager.GameEvent.EventType.ResumeGame)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = pauseVelocity;
        }
        else if(obj.myType == EventManager.GameEvent.EventType.NewWave)
        {
            if(collectable == null)
            {
                GameObject ob = GameObject.FindGameObjectWithTag("Collectables");
                collectable = ob.GetComponent<RectTransform>();
            }
        }
    }

    private void OnDisable()
    {
        EventManager.OnGameEvent -= onGameEvent;
    }

    public void Launched()
    {
        canBePressed = true;
		spriteRenderer.enabled = true;
		myStage = SpriteStage.Up;
        UpdateSprite();
    }

    public void OnGround()
    {
        myStage = SpriteStage.Up;
        WasDrawn = false;
        canBePressed = false;
        leftArm.SetActive(false);
        rightArm.SetActive(false);
    }

    private void Update()
    {
        if(rb.velocity.y < 0 && myStage != SpriteStage.Down)
        {
            myStage = SpriteStage.Down;
            UpdateSprite();
        }
    }

    public void OnPressed()
    {
        WasDrawn = true;
        canBePressed = false;
        myStage = SpriteStage.Connecting;

        UpdateSprite();
		myConnectingAnimator.enabled = true;

		//myAnimator.SetBool("Pressed", true);

		myConnectingAnimator.Play("Pressed_Demo", -1, 0);
		//Debug.Break();
	}

    public void SetLeftArm()
    {
        leftArm.SetActive(true);
    }

    public void SetRightArm()
    {
        leftArm.SetActive(true);
    }

    void UpdateSprite()
    {
        spriteRenderer.sprite = sprites[(int)myStage];
    }

    public void ConnectAnimationDone()
    {
		//myAnimator.SetBool("Pressed", false);
		myConnectingAnimator.enabled = false;
        leftArm.SetActive(false);
        rightArm.SetActive(false);
    }

    public void MoveToTarget()
    {
		myFlyAnimator.enabled = true;
		//myAnimator.SetBool("FlyAway", true);
		myFlyAnimator.Play("FlyAwayAnim", -1, 0);
		myFlyAnimator.speed = 1f;
  //      Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(collectable.transform.position.x, collectable.transform.position.y, 0f));
		//newPosition.z = 0;
		//iTween.MoveTo(gameObject, iTween.Hash(
		//	"path", GameManager.Instance.transformPath,
  //          "islocal", false,
		//	"orienttopath", true,
		//	"axis", "z",
		//	"lookahead", 1.0,

		//	"time", 0.5f,
		//	"easetype", iTween.EaseType.easeInExpo,
		//	"oncomplete", "moveComplete"
  //          ));
    }

    public void moveComplete()
    {
		//myAnimator.SetBool("FlyAway", false);
		//myFlyAnimator.Play("FlyAway", -1, 0);
		//myFlyAnimator.speed = 0f;
		myFlyAnimator.enabled = false;
		spriteRenderer.enabled = false;
		//ParticleSystem ps = collectable.GetChild(0).GetComponent<ParticleSystem>();
		//ps.gameObject.SetActive(true);
		//ps.Emit(100);
		Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(collectable.transform.position.x, collectable.transform.position.y, 0f));
		newPosition.z = 0;
		scoreParticles.transform.position = collectable.transform.position;
		scoreParticles.gameObject.SetActive(true);
		scoreParticles.Emit(100);
        SpawnerManager.Instance.ResetAllShapes();
        EventManager.Scored();
    }

    internal void ResetPosition()
    {
        transform.position = startPosition;
    }

	public void Print(string aMessage)
	{
		Debug.Log(aMessage);
	}
}
