using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// What I would like to have in a LoadingScreenManager:
//
// - A way to say: Yo open the loading screen now!
// - A way to show loading progress
// - A way to say I'm done loading
//

[RequireComponent(typeof(Animation))]
public class LoadingScreenManager : AGameStateSubscriber
{
    [System.Serializable]
    private class Definition
    {
        public GameState TriggerGameState;

        public UnityEvent OnStartLoading;
        public UnityEvent OnMidLoading;
        public UnityEvent OnEndLoading;
    }

    public event UnityAction OnFadeEnd;
    public event UnityAction OnMidLoading;
    
    [SerializeField]
    private Definition[] m_loadingDefitions;
    private GameState m_currentState = GameState.None;

    public void Fade()
    {
        m_animation.Play("A_Fade");
    }

    public void MidLoading()
    {
        for (int i = 0, c = m_loadingDefitions.Length; i < c; ++i)
        {
            if (m_currentState == m_loadingDefitions[i].TriggerGameState)
            {
                m_loadingDefitions[i].OnMidLoading?.Invoke();
            }
        }

        // OnMidLoading?.Invoke();
    }

    public void FadeEnd()
    {
        for (int i = 0, c = m_loadingDefitions.Length; i < c; ++i)
        {
            if (m_currentState == m_loadingDefitions[i].TriggerGameState)
            {
                m_loadingDefitions[i].OnEndLoading?.Invoke();
            }
        }

        // OnFadeEnd?.Invoke();
    }
    
    public override void OnGameStateChange(GameState newState)
    {
        if(m_currentState == newState || m_loadingDefitions == null)
        {
            return;
        }

        m_currentState = newState;

        for (int i = 0, c = m_loadingDefitions.Length; i < c; ++i)
        {
            if (m_currentState == m_loadingDefitions[i].TriggerGameState)
            {
                m_loadingDefitions[i].OnStartLoading?.Invoke();
            }
        }
    }

    private Animation m_animation;
    private void Awake()
    {
        m_animation = GetComponent<Animation>();
    }
}

