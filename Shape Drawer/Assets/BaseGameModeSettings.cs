using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class BaseGameModeSettings
{
	public int GoalAmout { set; get; }

	protected int ShapesReadyAmount { set; get; }
	public bool HasScoredThisWave { get; set; }
	public int ShapeReadyGoal
	{
		set;
		get;
	}

	public BaseGameModeSettings()
	{
		Reset();
	}

	public virtual void Reset()
	{
		ShapesReadyAmount = 0;
		GoalAmout = 0;
		HasScoredThisWave = false;
	}

	public abstract bool IsTimedLevel();

	public abstract bool ShouldShowInc();

	public virtual void CheckIfCanScore()
	{
		Debug.Log("CheckIfShouldScore is not overriden!!!!");
	}

	public void ShapesReadyToFly()
	{
		ShapesReadyAmount++;
		ShapesReady();
	}

	public virtual void ShapesReady()
	{
		Debug.LogError("ShapesReady is not overriden!!");
	}

	protected void ShapeReady(List<Shape> aShapeList)
	{
		for (int i = 0; i < aShapeList.Count; ++i)
		{
			if (i == 0)
			{
				aShapeList[i].MoveToTarget();
				HasScoredThisWave = true;
			}
			else
			{
				aShapeList[i].gameObject.SetActive(false);
			}
		}
	}

	public abstract void MouseUpAction();
}
