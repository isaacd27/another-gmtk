using System;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Extension/Pool Object")]
public class PoolObject : MonoBehaviour
{
	public event Action onEnable = null;
	public event Action onDisable = null;

	public int PrefabIndex
	{
		get; set;
	}

	public bool IsActive
	{
		get
		{
			return gameObject.activeSelf;
		}
		set
		{
			if (IsActive == value)
				return;

			gameObject.SetActive(value);
		}
	}

	public Vector3 Position
	{
		get
		{
			return transform.position;
		}
		set
		{
			transform.position = value;
		}
	}

	protected virtual void OnEnable()
	{
		onEnable?.Invoke();
	}

	protected virtual void OnDisable()
	{
		onDisable?.Invoke();
	}
}
