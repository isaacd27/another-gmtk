using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateLoadingBaseState : NobunAtelier.StateComponent<GameStateDefinition>
{
    protected abstract GameStateDefinition NextState { get; }

    public override void Enter()
    {
        LoadingScreenManager.Instance.AddLoaderToExecute(OnLoadReady);
        LoadingScreenManager.Instance.OnLoadingCompleted += OnLoadingCompleted;
        LoadingScreenManager.Instance.StartFadeIn();
    }

    private void OnLoadReady()
    {
        Load();
        LoadingScreenManager.Instance.StartFadeOut();
    }

    // Load next state.
    protected virtual void OnLoadingCompleted()
    {
        SetState(NextState);
    }

    protected virtual void Load() { }
}
