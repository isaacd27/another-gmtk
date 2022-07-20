using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateLoadingGame : GameStateLoadingBaseState
{
    protected override GameStateDefinition NextState => m_inGameState;
    [SerializeField]
    private GameStateDefinition m_inGameState;

    protected override void Load()
    {
        if (PlayerControllerManager.Instance)
        {
            PlayerControllerManager.Instance.ResetPlayer();
        }
        if (BattleWaveManager.Instance)
        {
            BattleWaveManager.Instance.ResetManager();
        }
    }
}
