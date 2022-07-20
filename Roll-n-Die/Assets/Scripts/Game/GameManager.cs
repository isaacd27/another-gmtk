using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum GameState
{
    // Passive - Waiting for callback
    None,

    // Engine init
    Init,

    // Splashscreen
    SplashScreen,

    // Menu
    // Player Input: UI only - Gamepad or Mouse
    FrontEnd,

    // FrontEnd/Scoreboard to InGame.
    // Player Input: Locked
    LoadingGame,    
    
    // Player Input: Player Controller - no UI
    InGame,
    // Player Input: UI only
    InGamePause,
    // Player Input: UI only
    GameOver,
    // Player Input: UI only
    ScoreBoard,

    LoadingMenu,
    LoadingScoreboard,
}



//public interface IPlayerController
//{
//    // Set to true to disable character control
//    void SetInputLocking(bool enable);
//}

[System.Serializable]
public class GameStateEvent : UnityEvent<GameState> { }

// GameManager script must be first to be executed in Script execution order!
public class GameManager : MonoBehaviour, IGameStateManager
{
    private static GameManager m_instance = null;
    public static GameManager Instance => m_instance;

    public GameStateEvent OnGameStateChange;
    public GameState GameState => m_currentGameState;
    private GameState m_currentGameState = GameState.None;

    [SerializeField]
    private GameState m_startingState = GameState.Init; 
    // [SerializeField]
    // private GameObject m_frontEndPanel = null;
    [SerializeField]
    private LoadingScreenManager m_fadingScreenManager = null;
    [SerializeField]
    private GameObject m_inGamePauseMenuPanel = null;
    [SerializeField]
    private GameObject m_HUD = null;
    [SerializeField]
    private GameObject m_gameOverPanel = null;
    [SerializeField]
    private GameObject m_scoreboardPanel = null;

    private List<IGameStateSubscriber> m_gameStateSubscribers = new List<IGameStateSubscriber>();
    private Dictionary<GameState, State> m_gameStates;


    public void Awake()
    {
        m_instance = this;

        // Debug.Assert(m_frontEndPanel != null);
        Debug.Assert(m_inGamePauseMenuPanel != null);
        Debug.Assert(m_fadingScreenManager != null);
        Debug.Assert(m_HUD != null);
        // m_frontEndPanel.SetActive(false);
        m_inGamePauseMenuPanel.SetActive(false);
        m_HUD.SetActive(false);

        Application.targetFrameRate = 60;

        m_gameStates = new Dictionary<GameState, State>(10);

        m_gameStates.Add(GameState.Init,            new InitState());
        m_gameStates.Add(GameState.SplashScreen,    new State());
        m_gameStates.Add(GameState.FrontEnd,        new FrontEndState());
        m_gameStates.Add(GameState.LoadingGame,     new State());
        m_gameStates.Add(GameState.InGame,          new State());
        m_gameStates.Add(GameState.InGamePause,     new State());
        m_gameStates.Add(GameState.GameOver,        new State());
        m_gameStates.Add(GameState.ScoreBoard,      new State());
    }

    private void Start()
    {
        SetGameState(m_startingState);
    }

    public void Register(IGameStateSubscriber gameStateSubscriber)
    {
        m_gameStateSubscribers.Add(gameStateSubscriber);
    }

    public void Unregister(IGameStateSubscriber gameStateSubscriber)
    {
        m_gameStateSubscribers.Remove(gameStateSubscriber);
    }

    public void SetGameState(GameState newState)
    {
        if (m_currentGameState == newState)
        {
            return;
        }

        // Exit current state.
        m_gameStates[m_currentGameState].Exit();
        // Notify new state.
        OnGameStateChange?.Invoke(m_currentGameState = newState);
        // Enter new state.
        m_gameStates[m_currentGameState].Enter();

        for (int i = 0, c = m_gameStateSubscribers.Count; i < c; ++i)
        {
            m_gameStateSubscribers[i].OnGameStateChange(m_currentGameState);
        }
    }

    public void SetGameStateToInGame()
    {
        SetGameState(GameState.InGame);
    }

    private void Update()
    {
        m_gameStates[m_currentGameState].Update();

//        switch (m_currentGameState)
//        {
//            //case GameState.Init:
//            //    SetGameState(GameState.SplashScreen);
//            //    break;
            
//            // The splash screen manager will change the game state when done
//            //case GameState.SplashScreen:
//            //    break;

//            // Wait for player instruction
//            //case GameState.FrontEnd:
//            //    // m_frontEndPanel.SetActive(true);

//            //    if (Input.anyKeyDown)
//            //    {
//            //        SetGameState(GameState.LoadingGame);
//            //    }
//            //    break;
            
//            // Stylish Fade in screen
//            //case GameState.LoadingGame:
//            //    // m_frontEndPanel.SetActive(false);
//            //    m_inGamePauseMenuPanel.SetActive(false);
//            //    if (m_scoreboardPanel)
//            //    {
//            //        m_scoreboardPanel.SetActive(false);
//            //    }

//            //    // loading...
//            //    // m_fadingScreenManager.Fade();
//            //    // m_fadingScreenManager.OnMidLoading += MidLoadingToInGame;
//            //    // m_fadingScreenManager.OnFadeEnd += LoadingToInGame;
//            //    // SetGameState(GameState.None);
//            //    break;

//            // Stylish fade out screen
//            // case GameState.InGame:
////                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButton("Pause"))
////                {
////                    Pause(m_currentGameState != GameState.InGamePause);
////                }

////#if UNITY_EDITOR
////                if (Input.GetKeyDown(KeyCode.L))
////                {
////                    GameOver();
////                }

////                if (Input.GetKeyDown(KeyCode.P))
////                {
////                    BattleWaveManager.Instance.StartNextWave();
////                }
////#endif
//                // break;

//            //case GameState.GameOver:
//            //    break;

//            //case GameState.ScoreBoard:
//            //    break;
//        }

    }
    
    private void HideAllScreen()
    {
        // m_frontEndPanel.SetActive(false);
        m_inGamePauseMenuPanel.SetActive(false);
        if (m_scoreboardPanel)
        {
            m_scoreboardPanel.SetActive(false);
        }
        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(false);
        }
    }

    /// /////////////////////////////////////////////
    // IN GAME
    private void MidLoadingToInGame()
    {
        m_fadingScreenManager.OnMidLoading -= MidLoadingToInGame;

        if (PlayerControllerManager.Instance)
        {
            PlayerControllerManager.Instance.ResetPlayer();
        }
        if (BattleWaveManager.Instance)
        {
            BattleWaveManager.Instance.ResetManager();
        }
    }

    private void LoadingToInGame()
    {
        m_fadingScreenManager.OnFadeEnd -= LoadingToInGame;
        m_HUD.SetActive(true);
        SetGameState(GameState.InGame);

        if (BattleWaveManager.Instance)
        {
            StartCoroutine(WaitAndExecute(1f, BattleWaveManager.Instance.StartNextWave));
        }
    }


    /// /////////////////////////////////////////////
    // PUBLIC CALLBACK
    public bool IsGamePaused => m_currentGameState == GameState.InGamePause;
    public void Pause(bool pause)
    {
        if(IsGamePaused == pause)
        {
            return;
        }

        if (pause)
        {
            SetGameState(GameState.InGamePause);
        }
        else if (!pause)
        {
            SetGameState(GameState.InGame);
        }
    }
    
    public void RestartGame()
    {
        SetGameState(GameState.LoadingGame);
    }

    public void BackToMenu()
    {
        // Kill all the entities - AI and Player
        m_inGamePauseMenuPanel.SetActive(false);
        PlayerControllerManager.Instance.SetInputLock(true);
        m_fadingScreenManager.Fade();
        m_fadingScreenManager.OnFadeEnd += LoadingToMenu;
    }


    /// /////////////////////////////////////////////
    // TO MENU
    private void LoadingToMenu()
    {
        m_fadingScreenManager.OnFadeEnd -= LoadingToMenu;
        SetGameState(GameState.FrontEnd);
    }


    /// /////////////////////////////////////////////
    // GAMEOVER
    public void GameOver()
    {
        // Do stuff with score...
        SetGameState(GameState.GameOver);
    }

    private void ConfirmGameOver()
    {
        SetGameState(GameState.LoadingScoreboard);

        // m_fadingScreenManager.OnMidLoading += GameOverMidLoadingToScoreboard;
        // m_fadingScreenManager.OnFadeEnd += GameOverLoadingToScoreboard;
        // m_fadingScreenManager.Fade();
    }

    /// /////////////////////////////////////////////
    // SCOREBOARD
    private void GameOverMidLoadingToScoreboard()
    {
        m_fadingScreenManager.OnMidLoading -= GameOverMidLoadingToScoreboard;
        if (m_scoreboardPanel)
        {
            m_scoreboardPanel.SetActive(true);
        }
    } 

    private void GameOverLoadingToScoreboard()
    {
        m_fadingScreenManager.OnFadeEnd -= GameOverLoadingToScoreboard;
        SetGameState(GameState.ScoreBoard);
        // Button on scoreboard to go back to menu or retry!
    }

    /// /////////////////////////////////////////////
    // OTHER
    public IEnumerator WaitAndExecute(float duration, UnityAction action)
    {
        yield return new WaitForSecondsRealtime(duration);
        action?.Invoke();
    }


#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(200));
        GUILayout.Label("<b>------------</b>");
        GUILayout.Label("<b>Game Manager</b>");
        UIDebugDrawLabelValue("Game State", m_currentGameState.ToString());

        GUILayout.Label("<b>------------</b>");
        GUILayout.Label("<b>Player Manager</b>");
        UIDebugDrawLabelValue("Input Lock", PlayerControllerManager.Instance.IsInputLock.ToString());

        if(BattleWaveManager.Instance)
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
        GUILayout.EndVertical();
    }
#endif

    private void UIDebugDrawLabelValue(string label, string value)
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label(label + ":");
            GUILayout.Label(value);
        }
        GUILayout.EndHorizontal();
    }



    private class State
    {
        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }

    private class InitState : State
    {
        public override void Enter() 
        {
            GameManager.Instance.SetGameState(GameState.SplashScreen);
        }
    }

    private class FrontEndState : State
    {
        public override void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                GameManager.Instance.SetGameState(GameState.LoadingGame);
            }
        }
    }

    private class LoadingGameState : State
    {
        public override void Enter()
        {
            GameManager.Instance.m_inGamePauseMenuPanel.SetActive(false);
            if (GameManager.Instance.m_scoreboardPanel)
            {
                GameManager.Instance.m_scoreboardPanel.SetActive(false);
            }
        }
    }

    private class InGameState : State
    {
        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButton("Pause"))
            {
                GameManager.Instance.Pause(GameManager.Instance.m_currentGameState != GameState.InGamePause);
            }

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.L))
            {
                GameManager.Instance.GameOver();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                BattleWaveManager.Instance.StartNextWave();
            }
#endif
        }
    }

    private class InGamePauseState : State
    {
        public override void Enter() 
        {
            GameManager.Instance.m_HUD.SetActive(false);
            PlayerControllerManager.Instance.SetInputLock(true);
            GameManager.Instance.m_inGamePauseMenuPanel.SetActive(true);
        }
        
        public override void Exit()
        {
            GameManager.Instance.m_HUD.SetActive(true);
            PlayerControllerManager.Instance.SetInputLock(false);
            GameManager.Instance.m_inGamePauseMenuPanel.SetActive(false);
        }
    }

    private class GameOverState : State
    {
        public override void Enter() 
        {
            PlayerControllerManager.Instance.SetInputLock(true);

            if (GameManager.Instance.m_gameOverPanel)
            {
                GameManager.Instance.m_gameOverPanel.SetActive(true);
            }
            GameManager.Instance.StartCoroutine(GameManager.Instance.WaitAndExecute(3f, GameManager.Instance.ConfirmGameOver));
        }

        public override void Exit()
        {
            if (GameManager.Instance.m_gameOverPanel)
            {
                GameManager.Instance.m_gameOverPanel.SetActive(false);
            }
        }
    }
}
