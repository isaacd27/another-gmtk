using UnityEngine;

public abstract class SingletonManager : MonoBehaviour
{
    public static SingletonManager Instance => m_instance;
    private static SingletonManager m_instance = null;

    public virtual void ManagerCreation() { }
    public abstract void StartManager();
    public abstract void PauseManager(bool isPaused);
    public abstract void ResetManager();

    public static T GetInstance<T>()
        where T: class
    {
        return m_instance as T;
    }


    private void Awake()
    {
        ManagerCreation();
        m_instance = this;
    }
}