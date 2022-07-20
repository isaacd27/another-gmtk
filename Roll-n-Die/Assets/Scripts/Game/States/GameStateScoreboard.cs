using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateScoreboard : NobunAtelier.StateComponent<GameStateDefinition>
{
    [SerializeField]
    private GameObject m_scoreboard;

    protected override void Start()
    {
        base.Start();

        Debug.Assert(m_scoreboard);
        m_scoreboard.SetActive(false);
    }
    public override void Enter()
    {
        m_scoreboard.SetActive(true);
    }

    public override void Exit()
    {
        m_scoreboard.SetActive(false);
    }
}
