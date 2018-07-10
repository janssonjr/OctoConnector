using System.Linq;
using UnityEngine;
public class ConnectAllGameMode : BaseGameModeSettings
{
	public override void CheckIfCanScore()
	{
		SpawnerManager spawner = SpawnerManager.Instance;
		int drawCount = spawner.shapeForced.Count(s => { return s.WasDrawn == true; });
		if (drawCount == spawner.shapeForced.Count)
		{
			EventManager.PreScore();
			spawner.shapeForced.ForEach(s => 
			{
				s.MoveToCenter();
			});
			GoalAmout = 1;
		}
	}

	public override bool IsTimedLevel()
	{
		return false;
	}

	public override void MouseUpAction()
	{
		SpawnerManager spawner = SpawnerManager.Instance;
		if (spawner.shapeForced.Count(s => { return s.WasDrawn == true; }) != spawner.shapeForced.Count)
		{
			foreach (var shape in spawner.shapeForced)
			{
				shape.WasDrawn = false;
				shape.canBePressed = true;
				shape.rb.bodyType = RigidbodyType2D.Dynamic;
			}
		}
	}

	public override void ShapesReady()
	{
		if (ShapesReadyAmount == SpawnerManager.Instance.shapeForced.Count && HasScoredThisWave == false)
		{
			var shapedForced = SpawnerManager.Instance.shapeForced;
			ShapeReady(shapedForced);
		}
	}

	public override bool ShouldShowInc()
	{
		return true;
	}
}