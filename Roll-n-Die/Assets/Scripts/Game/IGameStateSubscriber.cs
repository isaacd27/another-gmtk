public interface IGameStateSubscriber
{
    void OnGameStateChange(GameState newState);
}