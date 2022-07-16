using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FireRateEvent : UnityEvent<float>
{
}

public class Weapon : MonoBehaviour
{
    private readonly int IsometricRangePerYUnit = 100;
   
   protected float Lastfired;

    [SerializeField]
    protected float maxFireRate = 1.0f;

    private float fireRate = 1.0f;
	public float FireRate
	{
		get => fireRate;
		protected set
		{
			if (fireRate == value)
				return;

			fireRate = value;
			OnFireRateChanged?.Invoke(fireRate / maxFireRate);
		}
	}

    [SerializeField]
    protected PlayerMovementController2D IsoController;

    [Header("Weapon - RENDERING")]
    [SerializeField, Range(-1, 1)]
    float weaponDistanceFromPlayer = 1;
    [SerializeField, Range(-360, 360)]
    float bonusDegree = 90f;
    [SerializeField, Tooltip("Use this to offset the object slightly in front or behind the Target object")]
    private int TargetOffsetFactor = -50;

	public FireRateEvent OnFireRateChanged = new FireRateEvent();

	protected virtual void Awake()
	{
		FireRate = maxFireRate;
	}

	protected virtual void Update()
    {
        Vector2 normalizedDirection = PlayerMovementController2D.lastWantedDirection.normalized;
        transform.position = IsoController.transform.position + weaponDistanceFromPlayer * new Vector3(normalizedDirection.x, normalizedDirection.y);

        transform.eulerAngles = new Vector3(0,0, Mathf.Rad2Deg * Mathf.Atan2(normalizedDirection.x, -normalizedDirection.y) + bonusDegree);

        float offset = transform.position.y - IsoController.transform.position.y;
        GetComponent<Renderer>().sortingOrder = -(int)(IsoController.transform.position.y * IsometricRangePerYUnit) + (int)(offset * TargetOffsetFactor);
    }
}
