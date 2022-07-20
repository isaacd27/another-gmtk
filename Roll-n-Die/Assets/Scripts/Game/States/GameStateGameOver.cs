using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateGameOver : NobunAtelier.StateComponent<GameStateDefinition>
{
    [Header("---- Game State ----")]
    [SerializeField]
    private GameStateDefinition m_scoreBoardState;
    [SerializeField]
    private GameObject m_gameOverPanel;
    [SerializeField]
    private float m_GameOverPanelDuration = 3f;
    private float m_timeBuffer = 0f;

    protected override void Start()
    {
        base.Start();

        Debug.Assert(m_gameOverPanel);
        Debug.Assert(m_scoreBoardState);
        m_gameOverPanel.SetActive(false);
    }

    public override void Enter()
    {
        base.Enter();
        m_timeBuffer = 0;
        m_gameOverPanel.SetActive(true);
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        m_timeBuffer += deltaTime;

        if (m_timeBuffer >= m_GameOverPanelDuration)
        {
            SetState(m_scoreBoardState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        m_gameOverPanel.SetActive(false);
    }
}
