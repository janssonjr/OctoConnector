[System.Serializable]
public class LevelData
{
    public LevelType LevelType;
    public int Goal;
    public int Moves;
    public int LevelIndex;
    public float Time;
    public bool IsComplete;
    public bool IsUnlocked;
    public bool TimedLevel
    {
        get
        {
            return LevelType == LevelType.Timed;
        }
    }
}