using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class WeaponUpdateEvent : UnityEvent<WeaponType> { }

public class GunFace : MonoBehaviour
{
    public Projectile projPrefab;
    public Stake staPrefab;

    // private PlayerMain playerMain;
    [Header("STAKE")]
    public float stakecool = 0.5f;
    public float coolstake = 0.1f;
    private float m_currentStakeCD = 0f;

    [Header("SHOTGUN")]
    public int shotnumbul = 5;
    public float Shotcoolstart = 0.5f;
    public float shotgunSpread = 45;
    public float shotGunBulletMinVelFactor = 1;
    public float shotGunBulletMaxVelFactor = 5;
    private float m_currentShotgunCD = 0f;


    [Header("PISTOL")]
    public int pistolAmmo = 10;
    public float pistolcool = 1f;
    private float m_currentPistolCD = 0f;

    [Header("RIFLE")]
    public int rifleAmmo = 6;
    public float rifleCool = 2f;
    private float m_currentRifleCD = 0f;

    [Header("DICEGUN")]
    public float diceguncool = 1f;
    private float m_currentDiceGunCD = 0;

    [Header("Events")]
    public WeaponUpdateEvent OnPrimaryWeaponUpdate;
    public WeaponUpdateEvent OnSecondaryWeaponUpdate;

    [Header("DEBUG?")]
    public WeaponType Weapon = WeaponType.Stake;
    public WeaponType secondary;
    public WeaponType primary;

    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        SetWeapon(WeaponType.Stake);
        SetSecondary(WeaponType.Pistol);
    }

    public void setPammo(int Delta)
    {
        pistolAmmo += Delta;
    }

    public void setRammo(int Delta)
    {
        rifleAmmo += Delta;
    }

    public void SetWeapon(WeaponType weapon)
    {
        Weapon = weapon;
        primary = weapon;
        OnPrimaryWeaponUpdate?.Invoke(weapon);

        //playerMain.PlayerSwapAimNormal.SetWeapon(weapon);
        // OnWeaponChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSecondary(WeaponType weapon)
    {
        secondary = weapon;
        OnSecondaryWeaponUpdate?.Invoke(weapon);
    }


    public WeaponType GetWeapon()
    {
        return Weapon;
    }

    public void RollWeapon1()
    {
        WeaponType wepno;

        do
        {
            wepno = (WeaponType)(UnityEngine.Random.Range(0, (int)WeaponType.Count));
        } while (wepno == secondary);

        SetWeapon(wepno);
    }

    public void RollWeapon2()
    {
        WeaponType wepno;

        do
        {
            wepno = (WeaponType)(UnityEngine.Random.Range(0, (int)WeaponType.Count));
        } while (wepno == primary);

        SetSecondary(wepno);
    }

    // Update is called once per frame
    void Update()
    {
        // Grendaes = GameStateManager.Instance.getBombs();
        Vector3 Mouseposition = Input.mousePosition;

        Mouseposition = Camera.main.ScreenToWorldPoint(Mouseposition);

        Vector2 Direction = new Vector2(
            Mouseposition.x - transform.position.x,
             Mouseposition.y - transform.position.y
             ).normalized;


        transform.up = Direction;




        Debug.DrawRay(transform.position, transform.up * 3, Color.white);



        m_currentPistolCD -= Time.deltaTime;
        m_currentRifleCD -= Time.deltaTime;
        m_currentStakeCD -= Time.deltaTime;
        m_currentShotgunCD -= Time.deltaTime;
        m_currentDiceGunCD -= Time.deltaTime;

        if (!PlayerControllerManager.Instance.IsInputLock)
        {

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.F1))
            {
                RollWeapon1();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.F2))
            {
                RollWeapon2();
            }
            if (Input.GetAxis("Fire1") != 0)
            {
                //  if(Weapon == "Stake" || Weapon == "Grenade")
                //  {
                //  Weapon = "Pistol";
                //  }
                Weapon = primary;
                onShoot(Direction);
            }
            else if (Input.GetAxis("Fire2") != 0)
            {
                Weapon = secondary;
                onShoot(Direction);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Weapon = primary;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Weapon = secondary;
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Danger"))
        {
            GameManager.Instance.GameOver();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Danger"))
        {
            Debug.Log("hit");
        }
    }

    public void onShoot(Vector2 dir)
    {
        switch (Weapon)
        {
            case WeaponType.Stake:
                StakeShoot(dir);
                break;
            case WeaponType.Pistol:
                PistolShoot(dir);
                break;
            case WeaponType.Rifle:
                RifleShoot(dir);
                break;
            case WeaponType.Shotgun:
                ShotgunShoot(dir);
                break;
            case WeaponType.DiceGun:
                DiceGunShoot(dir);
                break;
            default:
                break;
        }
    }

    private void PistolShoot(Vector2 dir)
    {
        if (m_currentPistolCD <= 0f)
        {
            Projectile temp = GameObject.Instantiate(projPrefab, new Vector3(this.transform.position.x + dir.x, this.transform.position.y + dir.y), this.transform.rotation);

            temp.transform.position = this.transform.position + this.transform.up * 0.4f * Mathf.Sign(this.transform.localScale.x);
            // temp.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

            temp.setDirection(dir);
            m_currentPistolCD = pistolcool;
        }
    }

    private void ShotgunShoot(Vector2 dir)
    {
        if (m_currentShotgunCD <= 0f)
        {
            for (int i = 0; i < shotnumbul; i++)
            {
                Vector3 spawnLocation = new Vector3(this.transform.position.x + dir.x, this.transform.position.y + dir.y) + (transform.up * UnityEngine.Random.Range(0.01f, 0.1f));
                Projectile temp = GameObject.Instantiate(projPrefab, spawnLocation, Quaternion.identity);

                // Just because you love quaternion :3
                Quaternion rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-shotgunSpread, shotgunSpread));
                Vector3 newDir = rotation * transform.up;
                temp.setDirection(newDir, UnityEngine.Random.Range(shotGunBulletMinVelFactor, shotGunBulletMaxVelFactor));// Quaternion.AngleAxis(i * 360 / shotgunSpread, dir) * Vector2.one);
            }
            m_currentShotgunCD = Shotcoolstart;
        }
    }

    private void StakeShoot(Vector2 dir)
    {
        if (m_currentStakeCD <= 0)
        {
            Stake temp = Instantiate(staPrefab, new Vector3(this.transform.position.x + dir.x, this.transform.position.y + dir.y), this.transform.rotation);

            temp.transform.position = this.transform.position + transform.up * 0.4f * Mathf.Sign(this.transform.localScale.x);
            temp.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

            temp.setDirection(dir);
            m_currentStakeCD = stakecool;
        }
    }

    private void RifleShoot(Vector2 dir)
    {
        if (m_currentRifleCD <= 0f)
        {

            // Debug.Log("ran ");
            Projectile temp = GameObject.Instantiate(projPrefab, new Vector3(this.transform.position.x + dir.x, this.transform.position.y + dir.y), this.transform.rotation);

            temp.transform.position = this.transform.position + this.transform.up * 0.4f * Mathf.Sign(this.transform.localScale.x);
            // temp.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

            temp.setDirection(dir);

            m_currentRifleCD = rifleCool;
        }
    }

    private void DiceGunShoot(Vector2 dir)
    {
        if (m_currentDiceGunCD <= 0f)
        {

            int bulletnum;

            bulletnum = Mathf.RoundToInt(UnityEngine.Random.Range(1, 6));
            if (bulletnum == 1)
            {

                Projectile temp = GameObject.Instantiate(projPrefab, new Vector3(this.transform.position.x + dir.x, this.transform.position.y + dir.y), this.transform.rotation);

                temp.transform.position = this.transform.position + this.transform.up * 0.4f * Mathf.Sign(this.transform.localScale.x);
                // temp.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);


                temp.setDirection(dir);
                m_currentDiceGunCD = diceguncool;
                //anim.SetBool("Rollin", false);
                // anim.SetInteger("Rollno", bulletnum);
                //string debug = anim.GetInteger("Rollno").ToString();

            }
            else
            {

                for (int i = 0; i < bulletnum; i++)
                {
                    // Debug.Log("ran ");
                    Projectile temp = GameObject.Instantiate(projPrefab, new Vector3(this.transform.position.x + dir.x, this.transform.position.y + dir.y), this.transform.rotation);

                    temp.transform.position = this.transform.position + this.transform.up * 0.4f * Mathf.Sign(this.transform.localScale.x);
                    // temp.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
                    float angleoffset = 0;
                    temp.setDirection(dir * 360 / (angleoffset * i));

                    m_currentDiceGunCD = diceguncool;// * bulletnum; //move the comment to enable variable cooldown
                }
            }
        }
    }
}
