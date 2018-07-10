using System.Linq;

class ConnectAllTimedGameMode : ConnectAllGameMode
{
	public override void CheckIfCanScore()
	{
		base.CheckIfCanScore();
	}

	public override bool IsTimedLevel()
	{
		return true;
	}

	public override void ShapesReady()
	{
		base.ShapesReady();
	}

	public override bool ShouldShowInc()
	{
		return true;
	}
}
