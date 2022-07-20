using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMainMenu : NobunAtelier.StateComponent<GameStateDefinition>
{
    [Header("---- Game State ----")]
    [SerializeField]
    private GameStateDefinition m_loadingGameState;
    [SerializeField]
    private GameObject m_mainMenuGao;

    protected override void Start()
    {
        base.Start();
        m_mainMenuGao.SetActive(false);
    }

    public override void Enter()
    {
        m_mainMenuGao.SetActive(true);
    }

    public override void Exit()
    {
        m_mainMenuGao.SetActive(false);
    }

    public override void Tick(float dt)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetState(m_loadingGameState);
        }
    }
}
