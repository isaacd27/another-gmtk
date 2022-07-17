using UnityEngine;

public abstract class SingletonManager<T> : MonoBehaviour
    where T: class
{
    public static T Instance => m_instance;
    protected static T m_instance = null;

    // You need to set m_instance = this.
    public abstract void ManagerCreation();
    public abstract void StartManager();
    public abstract void PauseManager(bool isPaused);
    public abstract void ResetManager();

    private void Awake()
    {
        ManagerCreation();
    }
}