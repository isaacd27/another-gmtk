using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemySpawnerManager : PoolManager
{
    private EnemySpawnerManager m_instance = null;
    public EnemySpawnerManager Instance => m_instance;

    [SerializeField]
    private EnemyWavesData m_data;
    // Parent storing the PoolSpawnRadius.
    [SerializeField] 
    private Transform spawnPointsFolder = null;

    private float m_timeStamp;

    private void Awake()
    {
        m_instance = this;
        ResetManager();
    }

    private void Start()
    {
        EnemyPool[] enemyPools = FindObjectsOfType<EnemyPool>();
        
        PoolSpawnRadius[] spawnPoints = spawnPointsFolder.GetComponentsInChildren<PoolSpawnRadius>();

    }

    public void StartManager()
    {

    }

    public void PauseManager(bool isPaused)
    {

    }

    public void ResetManager()
    {

    }
}
