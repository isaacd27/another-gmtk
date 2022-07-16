using System.Collections.Generic;
using UnityEngine;

public abstract class Pool : MonoBehaviour
{
	[SerializeField]
	protected Transform m_folder = null;

	[SerializeField]
	protected GameObject[] m_prefabs = null;
	protected List<PoolObject> m_pool = new List<PoolObject>();

	protected event System.Action<PoolObject> onObjectCreation = null;

	/// <summary>
	/// Either calls SpawnObject or activate an object
	/// </summary>
	/// <param name="prefabIndex">Index of the object type</param>
	/// <returns>Whether an object was added</returns>
	protected virtual bool AddObject(int prefabIndex, Vector3 position)
	{
		if (m_pool.Count <= 0)
		{
			SpawnObject(prefabIndex, position);
			return true;
		}

		for (int i = 0; i < m_pool.Count; ++i)
		{
			PoolObject crtObj = m_pool[i];
			if (crtObj == null || crtObj.PrefabIndex != prefabIndex || crtObj.IsActive)
				continue;

			crtObj.Position = position;
			crtObj.IsActive = true;
			return true;
		}

		return false;
	}

	protected void SpawnObject(int index, Vector3 position)
	{
		GameObject prefab = m_prefabs[index];
		if (prefab == null)
			return;

		GameObject obj = Instantiate(prefab, position, Quaternion.identity, m_folder);

		PoolObject poolObj = obj.GetComponent<PoolObject>();
		if (!poolObj)
			return;

		poolObj.PrefabIndex = index;
		m_pool.Add(poolObj);

		onObjectCreation?.Invoke(poolObj);
	}

	public virtual void Reset()
	{
		if (m_pool.Count <= 0)
			return;

		for (int i = 0; i < m_pool.Count; ++i)
			m_pool[i].IsActive = false;
	}
}