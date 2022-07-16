using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animation))]
public class FadingScreenManager : MonoBehaviour
{
    public event UnityAction OnFadeEnd;
    public event UnityAction OnMidLoading;

    public void Fade()
    {
        m_animation.Play("A_Fade");
    }

    public void MidLoading()
    {
        OnMidLoading?.Invoke();
    }

    public void FadeEnd()
    {
        OnFadeEnd?.Invoke();
    }

    private Animation m_animation;
    private void Awake()
    {
        m_animation = GetComponent<Animation>();
    }
}

