using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateInGamePause : NobunAtelier.StateComponent<GameStateDefinition>
{
    [SerializeField]
    private GameStateDefinition m_inGameState;
    [SerializeField]
    private GameObject m_inGamePauseMenuPanel;

    protected override void Start()
    {
        base.Start();
        m_inGamePauseMenuPanel.SetActive(false);
    }

    public override void Enter()
    {
        m_inGamePauseMenuPanel.SetActive(true);
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (Input.GetButtonDown("Cancel"))
        {
            SetState(m_inGameState);
        }
    }

    public override void Exit()
    {
        m_inGamePauseMenuPanel.SetActive(false);
    }
}
