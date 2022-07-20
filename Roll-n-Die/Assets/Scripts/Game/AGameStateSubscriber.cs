using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AGameStateSubscriber: MonoBehaviour, IGameStateSubscriber
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
