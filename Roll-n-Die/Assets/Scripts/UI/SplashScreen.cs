using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [SerializeField]
    private SplashScreen m_nextSplashScreen;

    public void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.SplashScreen)
        {
            this.Present();
        }
    }

    public void Present()
    {
        gameObject.SetActive(true);

        Animation anim = GetComponent<Animation>();
        if (anim && !anim.isPlaying)
        {
            anim.Rewind();
            anim.Play();
        }
        else
        {
            gameObject.SetActive(false);
            Next();
        }
    }

    public void Next()
    {
        if (m_nextSplashScreen)
        {
            m_nextSplashScreen.Present();
        }
        else
        {
            FindObjectOfType<GameManager>().SetGameState(GameState.FrontEnd);
        }
    }
}
