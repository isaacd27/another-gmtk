using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class LoadingScreenManager : SingletonManager<LoadingScreenManager>
{
    public event UnityAction OnLoadingCompleted;
    private event UnityAction OnReadyToLoad;
    private Animator m_transition;

    protected override LoadingScreenManager GetInstance()
    {
        return this;
    }

    protected override void Constructor()
    {
        m_transition = GetComponent<Animator>();
    }

    public void StartFadeIn()
    {
        m_transition.SetTrigger("FadeIn");
    }

    public void StartFadeOut()
    {
        m_transition.SetTrigger("FadeOut");
    }

    // To be called by the Animation
    private void ReadyToLoad()
    {
        OnReadyToLoad?.Invoke();
        OnReadyToLoad = null;

        StartFadeOut();
    }

    // To be called by the Animation
    private void LoadCompleted()
    {
        OnLoadingCompleted?.Invoke();
        OnLoadingCompleted = null;
    }

    public void AddLoaderToExecute(UnityAction action)
    {
        OnReadyToLoad += action;
    }
}

//public interface ILoader
//{
//    // Must be implemented
//    UnityAction OnLoadingCompleted { get; set; }
//}

//public class GMTTKLoadingScreenManager : ALoadingScreenManager<GMTTKLoadingScreenManager>
//{

//    protected override GMTTKLoadingScreenManager GetInstance()
//    {
//        return this;
//    }

//    public void StartLoading(ILoader loader)
//    {
//        // ++m_loadInProgress;
//        // loader.OnLoadingCompleted += () =>
//        // {
//        //     --m_loadInProgress;
//        // };
//    }

//    private void OnLoaderCompletion()
//    {
//        // OnLoaderCompletion
//    }
//}