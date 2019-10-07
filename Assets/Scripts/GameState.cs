/// <summary>
/// Represents the current state of the game.
/// </summary>
public static class GameState
{
    // Has the player lost?
    public static bool Lost = false;
    
    // Whether or not the game is paused
    public static bool Paused = false;

    // We need this
    public static bool PauseMenuEnabled = true;

    // Can the player draw?
    public static bool CanDraw = false;

    // The current level
    public static int CurrentLevel = 1;

    // The current score
    public static int Score = 0;

    public static int HighScore = 0;

}
