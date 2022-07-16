public static class PlayerData
{
	public static bool FirstGame { get; private set; } = true;

	public static void SetFirstGame()
	{
		FirstGame = false;
	}
}
