using System;
using System.Collections.Generic;
using UnityEngine;

public class GunFace : MonoBehaviour
{
    public Projectile projPrefab;
    public Stake staPrefab;

    // private PlayerMain playerMain;
    [Header("STAKE")]
    public float stakecool = 0.5f;

    [Header("SHOTGUN")]
    public int shotnumbul = 5;
    public float Shotcoolstart = 0.5f;
    public float shotgunSpread = 45;
    public float shotGunBulletMinVelFactor = 1;
    public float shotGunBulletMaxVelFactor = 5;
    float shotcool = 5f; 


    [Header("PISTOL")]
    public int pistolAmmo = 10;
    public float pistolcool = 1f;

    [Header("RIFLE")]
    public int rifleAmmo = 6;
    public float rifleCool = 2f;

    [Header("DICEGUN")]
    public float diecool = 1f;
    public float maxdiecool;

    [Header("DEBUG?")]
    public string Weapon = "Stake";
    public string secondary;
    public string primary;

    public bool paused = false;
    private bool canUseShotgun;
    private bool canUseRifle = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void setPammo(int Delta)
    {
        pistolAmmo += Delta;
    }

    public void setRammo(int Delta)
    {
        rifleAmmo += Delta;
    }


    public void SetCanUseShotgun()
    {
        canUseShotgun = true;
        //SetWeapon(weaponShotgun);
    }

    public void SetCanUseRifle()
    {
        canUseRifle = true;
        //SetWeapon(weaponRifle);
    }

    public void SetWeapon(String weapon)
    {
        Weapon = weapon;
        primary = weapon;
        //playerMain.PlayerSwapAimNormal.SetWeapon(weapon);
        // OnWeaponChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSecondary(String weapon)
    {
        secondary = weapon;
    }


    public String GetWeapon()
    {
        return Weapon;
    }

    public void RollWeapon1()
    {
        int wepno = (UnityEngine.Random.Range(0, 5));
        //0 = "Pistol"
        // 1= "Shotgun"
        // 2 = "Stake"
        // 3 = "Rifle"
        // 4 = "Die"
        /*   switch (wepno)
           {
               case wepno == 0: 

           }*/

        if (wepno == 0)
        {
            //plz change to switchcase if u can
            SetWeapon("Pistol");
        }
        else if (wepno == 1)
        {
            SetWeapon("Shotgun");
        }
        else if (wepno == 2)
        {
            //i forgot how to use switchcases
            SetWeapon("Stake");

        }
        else if (wepno == 3)
        {
            SetWeapon("Rifle");
        }
        else
        {
            SetWeapon("Die");
        }
        Debug.Log("Primary: " + primary);

    }

    public void RollWeapon2()
    {
        while(secondary == primary)
        {
            int wepno = (UnityEngine.Random.Range(0, 5));
            //0 = "Pistol"
            // 1= "Shotgun"
            // 2 = "Stake"
            // 3 = "Rifle"
            // 4 = "Die"
            /*   switch (wepno)
               {
                   case wepno == 0: 

               }*/

            if (wepno == 0)
            {
                //plz change to switchcase if u can
                SetSecondary("Pistol");
            }
            else if (wepno == 1)
            {
                SetSecondary("Shotgun");
            }
            else if (wepno == 2)
            {
                //i forgot how to use switchcases
                SetSecondary("Stake");

            }
            else if (wepno == 3)
            {
                SetSecondary("Rifle");
            }
            else
            {
                SetSecondary("Die");
            }
            Debug.Log("Secondary: " + secondary);
        }


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



        pistolcool -= Time.deltaTime;
        rifleCool -= Time.deltaTime;
        stakecool -= Time.deltaTime;
        shotcool -= Time.deltaTime;
        diecool -= Time.deltaTime;

        if (!PlayerControllerManager.Instance.IsInputLock)
        {

            if (Input.GetKey(KeyCode.K))
            {
                RollWeapon1();
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
        if (Weapon == "Pistol")
        {
            if (pistolcool <= 0f)
            {

              

                    Projectile temp = GameObject.Instantiate(projPrefab, new Vector3(this.transform.position.x + dir.x, this.transform.position.y + dir.y), this.transform.rotation);

                    temp.transform.position = this.transform.position + this.transform.up * 0.4f * Mathf.Sign(this.transform.localScale.x);
                    // temp.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);


                    temp.setDirection(dir);
                    pistolcool = 1f;
                
            }



        }
        else if (Weapon == "Shotgun")
        {
            if (shotcool <= 0f)
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
                
                shotcool = Shotcoolstart;                
            }
        }
        else if (Weapon == "Stake")
        {
            if (stakecool <= 0)
            {
                Stake temp = Instantiate(staPrefab, new Vector3(this.transform.position.x + dir.x, this.transform.position.y + dir.y), this.transform.rotation);

                temp.transform.position = this.transform.position + transform.up * 0.4f * Mathf.Sign(this.transform.localScale.x);
                temp.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

                temp.setDirection(dir);
                stakecool = 0.5f;
            }

        }
        else if (Weapon == "Rifle")
        {
            if (rifleCool <= 0f)
            {

                // Debug.Log("ran ");
                Projectile temp = GameObject.Instantiate(projPrefab, new Vector3(this.transform.position.x + dir.x, this.transform.position.y + dir.y), this.transform.rotation);

                temp.transform.position = this.transform.position + this.transform.up * 0.4f * Mathf.Sign(this.transform.localScale.x);
                // temp.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

                temp.setDirection(dir);

                rifleCool = 0.5f;

            }


        }
        else if (Weapon == "Die")
        {

            if (diecool <= 0f)
            {

                int bulletnum;

                bulletnum = Mathf.RoundToInt(UnityEngine.Random.Range(1, 6));
                if (bulletnum == 1)
                {

                    Projectile temp = GameObject.Instantiate(projPrefab, new Vector3(this.transform.position.x + dir.x, this.transform.position.y + dir.y), this.transform.rotation);

                    temp.transform.position = this.transform.position + this.transform.up * 0.4f * Mathf.Sign(this.transform.localScale.x);
                    // temp.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);


                    temp.setDirection(dir);
                     diecool = maxdiecool;
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
                        temp.setDirection(dir * (Mathf.Deg2Rad * (angleoffset * i)));

                        diecool = maxdiecool;// * bulletnum; //move the comment to enable variable cooldown
                    }
                }
            }

            }
        }
}
