using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerControllerManager : MonoBehaviour
{
    private static PlayerControllerManager m_instance = null;
    public static PlayerControllerManager Instance => m_instance;

    public bool IsInputLock { private set; get; } = true;

    [SerializeField]
    private BasePlayerController2D[] playerControllers;
    [SerializeField]
    private Transform[] m_playerSpawnPoints;

    public void SetInputLock(bool lockInput)
    {
        IsInputLock = lockInput;
    }

    public void ResetPlayer()
    {
        for (int i = 0, c = playerControllers.Length; i < c; ++i)
        {
            playerControllers[i].ResetEntity(m_playerSpawnPoints[i].position);
        }
        
        IsInputLock = false;
    }

    private void Awake()
    {
        m_instance = this;
        
        playerControllers = FindObjectsOfType<BasePlayerController2D>();
        
        Debug.Assert(m_playerSpawnPoints != null);
        Debug.Assert(m_playerSpawnPoints.Length >= playerControllers.Length);
    }
}