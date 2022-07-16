using System;
using UnityEngine;

public class BulletPool : Pool
{
	public static BulletPool Instance { get; private set; } = null;

	private Action<Bullet> initialization = null;

	private void Awake()
	{
		Instance = this;
		onObjectCreation += OnBulletCreated;
	}

	public void Instantiate(int ammoType, Vector3 origin, Vector2 normalizedDirection)
	{
		Vector3 spawnPos = origin + 0.5f * new Vector3(normalizedDirection.x, normalizedDirection.y);
		initialization = (Bullet bullet) =>
		{
			bullet.SetRadius(bullet.script.Radius);

			Vector2 force = normalizedDirection * bullet.script.Speed;
			bullet.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
		};

		if (!AddObject(ammoType, spawnPos))
			SpawnObject(ammoType, spawnPos);

		initialization = null;
	}

	private void OnBulletCreated(PoolObject poolObj)
	{
		Bullet bullet = poolObj.GetComponent<Bullet>();
		initialization?.Invoke(bullet);

		poolObj.onEnable += () =>
		{
			initialization?.Invoke(bullet);
		};
	}
}
