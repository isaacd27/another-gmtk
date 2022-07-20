using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateLoadingBaseState : NobunAtelier.StateComponent<GameStateDefinition>
{
    protected abstract GameStateDefinition NextState { get; }

    public override void Enter()
    {
        ALoadingScreenManager.Instance.AddLoaderToExecute(OnLoadReady);
        ALoadingScreenManager.Instance.OnLoadingCompleted += OnLoadingCompleted;
        ALoadingScreenManager.Instance.StartFadeIn();
    }

    private void OnLoadReady()
    {
        Load();
        ALoadingScreenManager.Instance.StartFadeOut();
    }

    // Load next state.
    protected virtual void OnLoadingCompleted()
    {
        SetState(NextState);
    }

    protected virtual void Load() { }
}
