using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class ConnectAmountGameMode : BaseGameModeSettings
{
	public override void CheckIfCanScore()
	{
		SpawnerManager spawner = SpawnerManager.Instance;
		List<Shape> drawnShapes = spawner.shapeForced.FindAll(s => { return s.WasDrawn == true; });
		if (drawnShapes != null)
		{
			if (drawnShapes.Count > 0)
			{
				EventManager.PreScore();
				drawnShapes.ForEach(s =>
				{
					s.MoveToCenter();
				});
				GoalAmout = drawnShapes.Count;
			}
		}
	}

	public override bool IsTimedLevel()
	{
		return false;
	}

	public override void MouseUpAction()
	{
		CheckIfCanScore();
	}

	public override void ShapesReady()
	{
		SpawnerManager spawner = SpawnerManager.Instance;
		List<Shape> drawnShapes = spawner.shapeForced.FindAll(s => { return s.WasDrawn == true; });
		if (drawnShapes != null)
		{
			if (drawnShapes.Count > 1 && HasScoredThisWave == false)
			{
				ShapeReady(drawnShapes);
			}
		}
	}

	public override bool ShouldShowInc()
	{
		return true;
	}
}
