using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
	[SerializeField] private int[] scenes = null;

	public int GetRandomScene()
	{
		int index = Random.Range(0, scenes.Length);
		return scenes[index];
	}
}
