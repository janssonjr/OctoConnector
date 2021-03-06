﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Core;

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
    //public GameObject leftArm;
    //public GameObject rightArm;

    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;

	public ParticleSystem scoreParticles;

    Vector2 pauseVelocity;
    RectTransform collectable;
    Vector3 startPosition;
	//TweenerCore<Vector3, Path, PathOptions> flyTween;
	Sequence flySequence;
	int flyPathIndex = 0;
	const string MoveToCenterTweenName = "MoveToCenter";

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        EventManager.OnGameEvent += onGameEvent;
		myConnectingAnimator.enabled = false;
		myFlyAnimator.enabled = false;
	}

    private void onGameEvent(EventManager.GameEvent obj)
	{
		//if(obj.myType == EventManager.GameEvent.EventType.Win)
		//{
		//MoveToCenter();
		//}
		if (obj.myType == EventManager.GameEvent.EventType.PauseGame)
		{
			pauseVelocity = rb.velocity;
			rb.bodyType = RigidbodyType2D.Static;
		}
		else if (obj.myType == EventManager.GameEvent.EventType.ResumeGame)
		{
			rb.bodyType = RigidbodyType2D.Dynamic;
			Debug.Log("Shape.onGameEvent: Changing " + gameObject.name+ "to dynamic");

			rb.velocity = pauseVelocity;
		}
		else if (obj.myType == EventManager.GameEvent.EventType.NewWave)
		{
			if (collectable == null)
			{
				GameObject ob = GameObject.FindGameObjectWithTag("Collectables");
				collectable = ob.GetComponent<RectTransform>();
			}
		}
	}

	public void MoveToCenter()
	{
		if (WasDrawn == true)
		{
			iTween.MoveTo(rb.gameObject, iTween.Hash(
				"position", GameManager.Instance.transformPath[0].position,
				"time", 1f,
				"oncompletetarget", GameManager.Instance.gameObject,
				"oncomplete", "ShapeReady",
				"name", MoveToCenterTweenName
				));
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
		rb.bodyType = RigidbodyType2D.Static;
		Debug.Log("Shape.OnPressed: Changing "+gameObject.name+" to static!");
        UpdateSprite();
		myConnectingAnimator.enabled = true;

		myConnectingAnimator.Play("Pressed_Demo", -1, 0);
		//Debug.Break();
	}

    void UpdateSprite()
    {
        spriteRenderer.sprite = sprites[(int)myStage];
    }

    public void ConnectAnimationDone()
    {
		myConnectingAnimator.enabled = false;
    }

    public void MoveToTarget()
    {
		myFlyAnimator.enabled = true;
		myFlyAnimator.Play("FlyAwayAnim", -1, 0);
		myFlyAnimator.speed = 1f;
		StartFly();
	}

	public void StartFly()
	{
		var path = GameManager.Instance.path;
		path[3] = collectable.transform.position;//Camera.main.ScreenToWorldPoint(new Vector3(collectable.transform.position.x, collectable.transform.position.y, 0f));
		path[2].x = ((path[3] - transform.position) / 2 + path[1]).x;
		//path[1].x = ((path[2] - transform.position) /	2 + path[0]).x;
		GameManager.Instance.path = path;
		flySequence = DOTween.Sequence();
		flySequence.Append(transform.DOPath(path, 1, PathType.CatmullRom, PathMode.Sidescroller2D)
			.SetDelay(GameManager.Instance.delay)
			.OnComplete(moveComplete)
			//.SetSpeedBased(true)
			.SetEase(Ease.Linear));
		flySequence.Join(transform.DORotate(new Vector3(0, 0, -140f), 0.5f).SetSpeedBased());
		//flyTween = transform.DOPath(path, GameManager.Instance.tweenTime, PathType.CatmullRom, PathMode.Sidescroller2D)
		//	.SetDelay(GameManager.Instance.delay)
		//	.OnComplete(moveComplete)
		//	.SetSpeedBased(true)
		//	.SetEase(Ease.Linear)
		//	.OnWaypointChange(OnWaypointChange);

		//iTween.MoveTo(gameObject, iTween.Hash(
		//	"path", path,
		//	"islocal", false,
		//	"speed", GameManager.Instance.tweenTime,
		//	"delay", GameManager.Instance.delay,
		//	"easeype", iTween.EaseType.linear,
		//	"oncomplete", "moveComplete"
		//	));
		//Debug.Break();
	}

	public void moveComplete()
    {
		myFlyAnimator.enabled = false;
		spriteRenderer.enabled = false;

		EmitScoreParticles();
        SpawnerManager.Instance.ResetAllShapes();
		//EventManager.WaveComplete();
		GameManager.Instance.CheckLevelComplete();
		GetComponentInChildren<AnimationFunctions>().MovedToTarget();
		myStage = SpriteStage.Up;
		UpdateSprite();
	}

	void EmitScoreParticles()
	{
		scoreParticles.transform.position = collectable.transform.position;
		scoreParticles.gameObject.SetActive(true);
		scoreParticles.Emit(100);
	}

    public void ResetPosition()
    {
        transform.position = startPosition;
    }

	public void RemoveTween()
	{
		iTween.StopByName(gameObject, MoveToCenterTweenName);
	}
}
