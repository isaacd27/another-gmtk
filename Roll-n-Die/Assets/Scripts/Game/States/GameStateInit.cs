using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameStateInit : NobunAtelier.StateComponent<GameStateDefinition>
{
    [Header("---- Game State ----")]
    [SerializeField]
    [FormerlySerializedAs("nextState")]
    private GameStateDefinition m_splashscreen;

    public override void Enter()
    {
        this.ParentStateMachine.SetState(m_splashscreen);
    }
}
