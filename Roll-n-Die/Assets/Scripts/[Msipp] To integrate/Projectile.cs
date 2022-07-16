using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    Rigidbody2D r;
    public float speed = 5f;
    public bool paused = false;
     Vector2 velcsave;
    //[RequireComponent(Rigidbody2D)]
    //public Vector2 speed = new Vector2(5f, 0f);
    // public float timer = 3f;
    // Start is called before the first frame update

    private void Awake()
    {
        r = GetComponent<Rigidbody2D>();
        // GetComponent<GunFace>().onShoot += Projectile.onShoot;
    }

    public void pauseflip()
    {
        paused = !paused;
    }
    void Start()

    {
        //Destroy(this.gameObject, timer);
    }

    // Update is called once per frame
    void Update()
    {
      //  transform.Translate(Vector2.up * Time.deltaTime * speed);
      if (paused)
        {
            if(r.velocity != Vector2.zero)
            {
                velcsave = r.velocity;
            }
          
            r.velocity = Vector2.zero;
        }
        else
        {
            if (r.velocity == Vector2.zero)
            {
                 r.velocity = velcsave;
            }
        }
    }

    public void setDirection(Vector2 dir)
    {
        transform.up = dir;
       r.velocity = new Vector2(dir.x * speed, dir.y * speed);
        // speed = speed * dir;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // temp = collision.gameObject.GetComponent<Zombie>();
        if (collision.gameObject.CompareTag("Player") == false)
        {
            Projectile temp = collision.gameObject.GetComponent<Projectile>();
            if(temp == null)
            {
                Destroy(this.gameObject);
                Debug.Log(collision.gameObject);
            }


        }

        // {

        //}
    }

    void onShoot()
    {

    }
}
