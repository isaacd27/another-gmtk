using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerControllerManager : SingletonManager<PlayerControllerManager>
{
    public bool IsInputLock { private set; get; } = true;

    [SerializeField]
    private TopDownBasePlayerController[] playerControllers;
    [SerializeField]
    private Transform[] m_playerSpawnPoints;

    protected override PlayerControllerManager GetInstance()
    {
        return this;
    }

    private void Start()
    {
        m_instance = this;
        playerControllers = FindObjectsOfType<TopDownBasePlayerController>();

        Debug.Assert(m_playerSpawnPoints != null);
        Debug.Assert(m_playerSpawnPoints.Length >= playerControllers.Length);
    }

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
    }
}