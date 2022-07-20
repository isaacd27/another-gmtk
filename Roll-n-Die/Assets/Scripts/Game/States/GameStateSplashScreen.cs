using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateSplashScreen : NobunAtelier.StateComponent<GameStateDefinition>
{
    [Header("---- Game State ----")]
    [SerializeField]
    private SplashScreen m_splashScreen;

    public override void Enter()
    {
        m_splashScreen.Present();
    }
}
