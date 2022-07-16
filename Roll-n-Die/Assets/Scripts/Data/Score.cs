using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Score", menuName = "Score")]
public class Score : ScriptableObject
{
	[SerializeField] private int value = 0;

	public int Value => value;

	public event Action<int> OnValueChanged = null;

	public void Increment()
	{
		++value;
		OnValueChanged?.Invoke(value);
	}

	public void Reset()
	{
		value = 0;
	}
}
