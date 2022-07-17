using UnityEngine;

public class PoolSpawnRadius : MonoBehaviour
{
	[SerializeField] private float radius = 1.0f;
	public float Radius => radius;
	[SerializeField]
	private MapMarkers m_marker;
	public MapMarkers Marker => m_marker;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
