using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu()]
public class EnemyWavesData : ScriptableObject
{
    [SerializeField]
    private WaveInfo[] m_wavesInfos;
    // Smaller waves triggered by every roll
    [SerializeField]
    private WaveInfo[] m_bonusRollWavesInfo;

    public WaveInfo[] WavesInfos => m_wavesInfos;
    public WaveInfo[] BonusRollWavesInfo => m_bonusRollWavesInfo;
}

[System.Serializable]
public class WaveInfo
{
    // public int WaveLevel;
    // [Min(0)]
    // public float DurationInSeconds = 10;
    // public AnimationCurve GlobalSpawnRate;
    [FormerlySerializedAs("Enemies")]
    [FormerlySerializedAs("SpawnerDefinition")]
    public EnemyDataPerMarker[] SpawnerDefinitions;
}

[System.Serializable]
public class EnemyDataPerMarker
{
    public MapMarkers SpawnPointMarker;
    [FormerlySerializedAs("enemiesData")]
    [FormerlySerializedAs("EnemiesSpawnDefinition")]
    [FormerlySerializedAs("EnemySpawnDefinition")]
    public EnemyWaveData[] EnemySpawnDefinitions;
}

[System.Serializable]
public class EnemyWaveData
{
    public PoolObjectID objectsID;
    public int MinNumberPerFrame;
    public int MaxCountPerFrame;
    // For instance we can have more of one type of enemy at the begining of the frame.
    // public AnimationCurve SpawnRateOverTheWave;
}