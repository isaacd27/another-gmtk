using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum GameState
{
    // Passive - Waiting for callback
    Idle,

    // Engine init
    Init,

    // Splashscreen
    SplashScreen,

    // Menu
    // Player Input: UI only - Gamepad or Mouse
    FrontEnd,
    // Player Input: UI only
    Settings,

    // FrontEnd/Scoreboard to InGame.
    // Player Input: Locked
    LoadingGame,    
    
    // Player Input: Player Controller - no UI
    InGame,
    // Player Input: UI only
    Pause,
    // Player Input: UI only
    GameOver,
    // Player Input: UI only
    ScoreBoard,
}

public interface IPlayerController
{
    // Set to true to disable character control
    void SetInputLocking(bool enable);
}

[System.Serializable]
public class GameStateEvent : UnityEvent<GameState> { }

// Gamemanager
public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;
    public static GameManager Instance => m_instance;

    public GameStateEvent OnGameStateChange;
    public GameState GameState => m_gameState;
    private GameState m_gameState;

    [SerializeField]
    private GameObject m_frontEndPanel = null;
    [SerializeField]
    private FadingScreenManager m_fadingScreenManager = null;
    [SerializeField]
    private GameObject m_inGamePauseMenuPanel = null;

    [SerializeField]
    private GameObject m_gameOverPanel = null;
    [SerializeField]
    private GameObject m_scoreboardPanel = null;

    public void Awake()
    {
        Debug.Assert(m_frontEndPanel != null);
        Debug.Assert(m_inGamePauseMenuPanel != null);
        Debug.Assert(m_fadingScreenManager != null);
        m_frontEndPanel.SetActive(false);
        m_inGamePauseMenuPanel.SetActive(false);

        m_gameState = GameState.Init;
        Application.targetFrameRate = 60;
        m_instance = this;

        DontDestroyOnLoad(this);
    }

    public void SetGameState(GameState newState)
    {
        OnGameStateChange?.Invoke(m_gameState = newState);
    }

    private void Update()
    {
        switch (m_gameState)
        {
            case GameState.Init:
                SetGameState(GameState.SplashScreen);
                break;
            
            // The splash screen manager will change the game state when done
            case GameState.SplashScreen:
                break;

            // Wait for player instruction
            case GameState.FrontEnd:
                m_frontEndPanel.SetActive(true);

                if (Input.anyKeyDown)
                {
                    SetGameState(GameState.LoadingGame);
                }
                break;
            
            // Stylish Fade in screen
            case GameState.LoadingGame:
                m_frontEndPanel.SetActive(false);
                m_inGamePauseMenuPanel.SetActive(false);
                if (m_scoreboardPanel)
                {
                    m_scoreboardPanel.SetActive(false);
                }

                // loading...
                m_fadingScreenManager.Fade();
                m_fadingScreenManager.OnMidLoading += MidLoadingToInGame;
                m_fadingScreenManager.OnFadeEnd += LoadingToInGame;
                SetGameState(GameState.Idle);
                break;

            // Stylish fade out screen
            case GameState.InGame:
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButton("Pause"))
                {
                    Pause(m_gameState != GameState.Pause);
                }

#if UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.L))
                {
                    GameOver();
                }

                if (Input.GetKeyDown(KeyCode.P))
                {
                    BattleWaveManager.Instance.StartNextWave();
                }
#endif
                break;

            case GameState.GameOver:
                //m_fadingScreenManager.OnMidLoading += MidLoadingToInGame;
                //m_fadingScreenManager.OnFadeEnd += LoadingToInGame;
                break;

            case GameState.ScoreBoard:
                break;
        }

    }
    
    private void HideAllScreen()
    {
        m_frontEndPanel.SetActive(false);
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
        PlayerControllerManager.Instance.ResetPlayer();

        BattleWaveManager.Instance.ResetManager();
    }

    private void LoadingToInGame()
    {
        m_fadingScreenManager.OnFadeEnd -= LoadingToInGame;
        SetGameState(GameState.InGame);
    }


    /// /////////////////////////////////////////////
    // PUBLIC CALLBACK
    public void Pause(bool pause)
    {
        if (m_gameState != GameState.Pause && pause)
        {
            SetGameState(GameState.Pause);
            PlayerControllerManager.Instance.SetInputLock(true);
            m_inGamePauseMenuPanel.SetActive(true);
        }
        else if (m_gameState == GameState.Pause && !pause)
        {
            SetGameState(GameState.InGame);
            PlayerControllerManager.Instance.SetInputLock(false);
            m_inGamePauseMenuPanel.SetActive(false);
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
        PlayerControllerManager.Instance.SetInputLock(true);

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }

        SetGameState(GameState.GameOver);

        StartCoroutine(WaitAndExecute(3f, ConfirmGameOver));
    }

    private void ConfirmGameOver()
    {
        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(false);
        }
        m_fadingScreenManager.OnMidLoading += GameOverMidLoadingToScoreboard;
        m_fadingScreenManager.OnFadeEnd += GameOverLoadingToScoreboard;
        m_fadingScreenManager.Fade();
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

    private void OnGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(200));
        GUILayout.Label("<b>------------</b>");
        GUILayout.Label("<b>Game Manager</b>");
        UIDebugDrawLabelValue("Game State", m_gameState.ToString());
        UIDebugDrawLabelValue("Player Input Lock", PlayerControllerManager.Instance.IsInputLock.ToString());
        GUILayout.Label("<b>------------</b>");
        GUILayout.Label("<b>DEBUG KEYS</b>");
        GUILayout.Label("<b>P</b>: Start next wave");
        GUILayout.Label("<b>L</b>: Force GameOver");
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
}
