using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [SerializeField]
    private SplashScreen m_nextSplashScreen;
    [SerializeField]
    public UnityEvent OnSplashScreenSequenceCompletion;


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
            OnSplashScreenSequenceCompletion?.Invoke();
            // FindObjectOfType<GameManager>().SetGameState(GameState.FrontEnd);
        }
    }
}
