using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// Need to add a way to pause wave?
public class BattleWaveManager : SingletonManager<BattleWaveManager>
{
    [SerializeField]
    private EnemyWavesData m_data;
    
    private EnemyPoolManager m_pool;
    public int CurrentWaveIndex => m_currentWaveIndex;
    private int m_currentWaveIndex = 0;

    public UnityEvent OnFinalWaveCompleted;

    protected override BattleWaveManager GetInstance()
    {
        return this;
    }

    public override void StartManager()
    {
    }

    public override void PauseManager(bool isPaused)
    {

    }

    public override void ResetManager()
    {
        m_currentWaveIndex = 0;

        EnemyPoolManager.Instance.ResetManager();
    }

    [ContextMenu("Start Next Wave")]
    public void StartNextWave()
    {
        ++m_currentWaveIndex;
        if (m_currentWaveIndex - 1 >= m_data.WavesInfos.Length)
        {
            OnFinalWaveCompleted?.Invoke();
            return;
        }

        WaveInfo cwave = m_data.WavesInfos[m_currentWaveIndex - 1];

        EnemyPoolManager.Instance.StartWave(cwave.SpawnerDefinitions);
    }
}
