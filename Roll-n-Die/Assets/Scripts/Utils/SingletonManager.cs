using UnityEngine;

public abstract class SingletonManager : MonoBehaviour
{
    public static SingletonManager Instance => m_instance;
    private static SingletonManager m_instance = null;

    public abstract void StartManager();
    public abstract void PauseManager(bool isPaused);
    public abstract void ResetManager();

    protected virtual void Awake()
    {
        m_instance = this;
    }
}