using UnityEngine;

public abstract class GameSingletonManager<T> : SingletonManager<T>, IGameStateSubscriber
    where T: class
{
    protected virtual void OnEnable()
    {
        Subscribe();
    }

    protected virtual void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        GameManager.Instance.Register(this);
    }

    private void Unsubscribe()
    {
        GameManager.Instance.Unregister(this);
    }

    public abstract void OnGameStateChange(GameState newState);
}




