using UnityEngine;
using UnityEngine.Events;

public class FiringWeapon : Weapon
{

    [SerializeField]
    public VacuumWeapon Vacuum;
    public Vector3 SpawnBulletOffset = new Vector3(0, 0, -1);

    public UnityEvent OnFiringStart;
    public UnityEvent OnFiringCharge;
    public UnityEvent OnShoot;
    public UnityEvent OnHeavyShoot;

    [SerializeField]
    private float WeaponChargeMax = 1.0f;
    [HideInInspector]
    public float WeaponCharge;

    // Update is called once per frame
    protected override void Update() 
    {
        base.Update();

        if (Input.GetAxis("Fire1") == 1 && !Vacuum.IsWorking)
        {
            WeaponCharge += Time.deltaTime;
            OnFiringCharge?.Invoke();
        }

        if(Input.GetAxis("Fire1") == 0 && !Vacuum.IsWorking)
        {
            if (WeaponCharge >= WeaponChargeMax)
            {
                OnHeavyShoot?.Invoke();
                Shoot(1);
            }
            else if(WeaponCharge > 0 && WeaponCharge < WeaponChargeMax)
            {
                OnShoot?.Invoke();
                Shoot(0);
            }
            WeaponCharge = 0.0f;
        }
    }

    public void Shoot(int AmmoType)
    {
        IsoController.SwitchWeapon(0);
        OnFiringStart?.Invoke();

		// BulletPool.Instance.Instantiate(AmmoType, IsoController.transform.position + SpawnBulletOffset, PlayerMovementController2D.lastWantedDirection.normalized);
    }
}
