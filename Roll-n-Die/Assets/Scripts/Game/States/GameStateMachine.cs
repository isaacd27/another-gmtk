using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : NobunAtelier.StateMachineComponent<GameStateDefinition>
{
    [Header("---- GMTK 2022 State Manager ----")]
    [SerializeField]
    private GameStateDefinition m_loadingGameState;
    [SerializeField]
    private GameStateDefinition m_loadingMenuState;

    protected override void Start()
    {
        Application.targetFrameRate = 60;
        base.Start();
        StartCoroutine(StartWithDelay_Coroutine());
    }

    private void Update()
    {
        Tick(Time.deltaTime);

        if(Input.GetButtonDown("Cancel"))
        {
            Debug.Log($"{Time.frameCount} - Button cancel is down!");
        }
    }

    public void RestartGame()
    {
        Debug.Assert(m_loadingGameState);
        SetState(m_loadingGameState);
    }

    public void BackToMenu()
    {
        Debug.Assert(m_loadingMenuState);
        SetState(m_loadingMenuState);
    }


    IEnumerator StartWithDelay_Coroutine()
    {
        yield return new WaitForSeconds(1f);
        Enter();
    }
}
