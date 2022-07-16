using UnityEngine;

public class PoolSpawnRadius : MonoBehaviour
{
	[SerializeField] private float radius = 1.0f;

	public float Radius => radius;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
