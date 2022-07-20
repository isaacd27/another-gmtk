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

    [SerializeField]
    private bool m_displayStatesDebug = false;
    private Vector2 scrollPos;

    protected override void Start()
    {
        Application.targetFrameRate = 60;
        base.Start();
        StartCoroutine(StartWithDelay_Coroutine());
    }

    private void Update()
    {
        Tick(Time.deltaTime);
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

#if UNITY_EDITOR
    protected override void OnGUI()
    {
        if(!m_displayDebug)
        {
            return;
        }
        
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("<b>------------</b>");
        GUILayout.Label("<b>Game Manager</b>");
        UIDebugDrawLabelValue("Game State", CurrentStateDefinition.name);

        GUILayout.Label("<b>------------</b>");
        GUILayout.Label("<b>Player Manager</b>");
        UIDebugDrawLabelValue("Input Lock", PlayerControllerManager.Instance.IsInputLock.ToString());

        if (BattleWaveManager.Instance)
        {
            GUILayout.Label("<b>------------</b>");
            GUILayout.Label("<b>Wave Manager</b>");
            UIDebugDrawLabelValue("Current Wave", BattleWaveManager.Instance.CurrentWaveIndex.ToString());

            if (EnemyPoolManager.Instance)
            {
                UIDebugDrawLabelValue("Remaining Enemies", EnemyPoolManager.Instance.RemainingEnemies.ToString());
            }
        }


        GUILayout.Label("<b>------------</b>");
        GUILayout.Label("<b>DEBUG KEYS</b>");
        GUILayout.Label("<b>P</b>: Start next wave");
        GUILayout.Label("<b>L</b>: Force GameOver");
        GUILayout.Label("<b>K</b>: Randomize Weapons");

        GUILayout.Label("<b>------------</b>");
        m_displayStatesDebug = GUILayout.Toggle(m_displayStatesDebug, "<b>State Machine</b>", GUI.skin.button);
        if (m_displayStatesDebug)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUI.skin.box);
            base.OnGUI();
            GUILayout.EndScrollView();
        }
        GUILayout.EndVertical();
    }

    private void UIDebugDrawLabelValue(string label, string value)
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label(label + ":");
            GUILayout.Label(value);
        }
        GUILayout.EndHorizontal();
    }
#endif
}
