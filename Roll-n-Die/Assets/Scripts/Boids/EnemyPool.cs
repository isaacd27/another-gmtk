using UnityEngine;
using UnityEngine.Events;

public class EnemyPool : Pool
{
	[SerializeField] private int initialCount = 0;
	[SerializeField] private Transform spawnPointsFolder = null;

	private int enemyCount = 0;
	public UnityEvent OnAllEnemyDisabled = new UnityEvent();

	private void Awake()
	{
		onObjectCreation += OnEnemyCreated;
		PoolSpawnRadius[] spawnPoints = spawnPointsFolder.GetComponentsInChildren<PoolSpawnRadius>();

		for (int i = 0; i < initialCount; ++i)
		{
			PoolSpawnRadius spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
			Vector2 circlePos = Random.insideUnitCircle * spawnPoint.Radius;
			Vector3 spawnPos = new Vector3(circlePos.x, circlePos.y, 0.0f) + spawnPoint.transform.position;
			SpawnObject(Random.Range(0, m_prefabs.Length), spawnPos);
		}
	}

	private void OnEnemyCreated(PoolObject poolObj)
	{
		++enemyCount;
		poolObj.onDisable += () =>
		{
			--enemyCount;
			if (enemyCount == 0)
				OnAllEnemyDisabled.Invoke();
		};
	}
}
