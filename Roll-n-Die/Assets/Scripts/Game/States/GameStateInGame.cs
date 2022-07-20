using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateInGame : NobunAtelier.StateComponent<GameStateDefinition>
{
    [Header("---- Game State ----")]
    [SerializeField]
    private GameStateDefinition m_pauseState;
    [SerializeField]
    private GameStateDefinition m_gameOverState;
    [SerializeField]
    private GameObject m_HUDGap;

    protected override void Start()
    {
        base.Start();
        Debug.Assert(m_HUDGap);
        m_HUDGap.SetActive(false);
    }

    public override void Enter()
    {
        m_HUDGap.SetActive(true);

        if (BattleWaveManager.Instance && BattleWaveManager.Instance.CurrentWaveIndex == 0)
        {
            BattleWaveManager.Instance.StartNextWave();
        }

        if (PlayerControllerManager.Instance)
        {
            PlayerControllerManager.Instance.SetInputLock(false);
        }
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Pause"))
        {
            SetState(m_pauseState);
            // GameManager.Instance.Pause(GameManager.Instance.m_currentGameState != GameState.InGamePause);
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L))
        {
            SetState(m_gameOverState);
            // GameManager.Instance.GameOver();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (BattleWaveManager.Instance)
            {
                BattleWaveManager.Instance.StartNextWave();
            }
        }
#endif
    }

    public override void Exit()
    {
        m_HUDGap.SetActive(false);

        if (PlayerControllerManager.Instance)
        {
            PlayerControllerManager.Instance.SetInputLock(true);
        }
    }
}
