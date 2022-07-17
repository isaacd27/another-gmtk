using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EnemyPoolManager : PoolManager<EnemyPoolManager>
{
	// [SerializeField] private int initialCount = 0;
	// parent of PoolSpawnRadius
	[SerializeField] private Transform spawnPointsParent = null;

	private int enemyCount = 0;
	public int RemainingEnemies => enemyCount;
    // Callback once all enemy of the spawner have been killed.
    public UnityEvent OnAllEnemyDisabled = new UnityEvent();
	
	[SerializeField]
	private Dictionary<MapMarkers, PoolSpawnRadius> m_spawnPointPerMarker;

#if UNITY_EDITOR
	[Header("**DEBUG**")]
	[TextArea]
	private string m_debugString;
#endif

    public override void ManagerCreation() 
    {
        m_instance = this;
    }

public void StartWave(EnemyDataPerMarker[] data)
    {
        foreach (var d in data)
        {
            if (!m_spawnPointPerMarker.ContainsKey(d.SpawnPointMarker))
            {
                Debug.LogError("d.SpawnPointMaker isn't available!");
                Debug.Break();
                return;
            }

            PoolSpawnRadius spawnPoint = m_spawnPointPerMarker[d.SpawnPointMarker];

            for (int i = 0, c = d.EnemySpawnDefinitions.Length; i < c; ++i)
            {
                EnemyWaveData ewd = d.EnemySpawnDefinitions[i];
                int count = Random.Range(ewd.MinNumberPerFrame, ewd.MaxCountPerFrame);
                SpawnObject(ewd.objectsID, spawnPoint.transform.position, spawnPoint.Radius, count);
            }
        }

        //for (int i = 0; i < initialCount; ++i)
        //{
        //    PoolSpawnRadius spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        //    Vector2 circlePos = Random.insideUnitCircle * spawnPoint.Radius;
        //    Vector3 spawnPos = new Vector3(circlePos.x, circlePos.y, 0.0f) + spawnPoint.transform.position;
        //    SpawnObject(Random.Range(0, m_prefabs.Length), spawnPos);
        //}
    }

    

    protected override void OnPoolManagerStart()
    {
        Debug.Assert(spawnPointsParent != null);
        base.OnPoolManagerStart();
    }

    protected override void OnObjectSpawned(IPoolableObject obj)
    {
        ++enemyCount;
    }

    protected override void OnPoolManagerReset()
    {

#if UNITY_EDITOR
        m_debugString = "";
#endif
		PoolSpawnRadius[] spawnPoints = spawnPointsParent.GetComponentsInChildren<PoolSpawnRadius>();

        if (spawnPoints == null)
        {
            Debug.LogError("Please assign spawn points!");
            Debug.Break();
        }

		m_spawnPointPerMarker = new Dictionary<MapMarkers, PoolSpawnRadius>(spawnPoints.Length);
		foreach (var sp in spawnPoints)
        {
			m_spawnPointPerMarker.Add(sp.Marker, sp);
#if UNITY_EDITOR
			m_debugString += $"- {sp.Marker} marked\n";
#endif
		}
	}

    protected override void OnObjectCreation(IPoolableObject poolObj)
    {
        poolObj.onDeactivation += () =>
        {
            --enemyCount;
            if (enemyCount == 0)
                OnAllEnemyDisabled.Invoke();
        };
    }
}

//public class EnemyPool : Pool
//{
//	[SerializeField] private int initialCount = 0;
//	[SerializeField] private Transform spawnPointsFolder = null;

//	private int enemyCount = 0;
//	// Callback once all enemy of the spawner have been killed.
//	public UnityEvent OnAllEnemyDisabled = new UnityEvent();

//	private void Awake()
//	{
//		onObjectCreation += OnEnemyCreated;
//		PoolSpawnRadius[] spawnPoints = spawnPointsFolder.GetComponentsInChildren<PoolSpawnRadius>();

//		for (int i = 0; i < initialCount; ++i)
//		{
//			PoolSpawnRadius spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
//			Vector2 circlePos = Random.insideUnitCircle * spawnPoint.Radius;
//			Vector3 spawnPos = new Vector3(circlePos.x, circlePos.y, 0.0f) + spawnPoint.transform.position;
//			SpawnObject(Random.Range(0, m_prefabs.Length), spawnPos);
//		}
//	}

    //private void OnEnemyCreated(PoolObject poolObj)
    //{
    //    ++enemyCount;
    //    poolObj.onDisable += () =>
    //    {
    //        --enemyCount;
    //        if (enemyCount == 0)
    //            OnAllEnemyDisabled.Invoke();
    //    };
    //}
//}