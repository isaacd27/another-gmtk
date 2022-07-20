public interface IGameStateManager
{
    void Register(IGameStateSubscriber sub);
    void Unregister(IGameStateSubscriber sub);
}