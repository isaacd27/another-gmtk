using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class GameStateResponder : AGameStateSubscriber
{
    [SerializeField]
    private GameState[] m_gameStatesToRespond;
    [FormerlySerializedAs("OnState")]
    public UnityEvent OnNewState;

    public override void OnGameStateChange(GameState newState)
    {
        for (int i = 0, c = m_gameStatesToRespond.Length; i < c; ++i)
        {
            if (newState == m_gameStatesToRespond[i])
            {
                OnNewState?.Invoke();
            }
        }
    }
}
