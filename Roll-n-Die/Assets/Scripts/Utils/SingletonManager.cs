using UnityEngine;

public abstract class SingletonManager<T> : MonoBehaviour
    where T: class
{
    public static T Instance => m_instance;
    protected static T m_instance = null;

    // You need to set m_instance = this.
    public abstract void ManagerCreation();
    public virtual void StartManager() { }
    public virtual void PauseManager(bool isPaused) { }
    public virtual void ResetManager() { }

    private void Awake()
    {
        ManagerCreation();
    }
}