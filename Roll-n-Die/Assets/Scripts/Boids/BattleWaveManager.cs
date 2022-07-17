using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BattleWaveManager : SingletonManager
{
    [SerializeField]
    private EnemyWavesData m_data;
    
    private EnemyPoolManager m_pool;
    private int m_currentWaveIndex = 0;

    public override void ManagerCreation()
    {
        m_currentWaveIndex = 0;
    }

    public override void StartManager()
    {
        m_pool = EnemyPoolManager.GetInstance<EnemyPoolManager>();
    }

    public override void PauseManager(bool isPaused)
    {

    }

    public override void ResetManager()
    {
        m_currentWaveIndex = 0;
        m_pool.ResetManager();
    }

    [ContextMenu("Start Next Wave")]
    public void StartNextWave()
    {
        ++m_currentWaveIndex;
        if (m_currentWaveIndex - 1 >= m_data.WavesInfos.Length)
        {
            GameManager.Instance.GameOver();
            return;
        }

        WaveInfo cwave = m_data.WavesInfos[m_currentWaveIndex - 1];
        m_pool.StartWave(cwave.Enemies);
    }
}
