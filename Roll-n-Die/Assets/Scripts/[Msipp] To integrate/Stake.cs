using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stake : MonoBehaviour
{
    PlayerController r;
    // Start is called before the first frame update
    public float speed =  0f;
    // public float timer = 3f;
    // Start is called before the first frame update

    private void Awake()
    {
        //r = GetComponent<Rigidbody2D>();
        // GetComponent<GunFace>().onShoot += Projectile.onShoot;
    }
    void Start()

    {
       Destroy(this.gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (r != null)
        {
           // this.transform.position = r.transform.position;
        }
        //transform.Translate(Vector2.up * Time.deltaTime * speed);
    }

    public void setDirection(Vector2 dir)
    {
        transform.up = dir;
      //  r.velocity = new Vector2(dir.x * speed, dir.y * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Danger"))
        {
            Destroy(this.gameObject);
        }else if (collision.gameObject.CompareTag("Player"))
        {
            r = collision.gameObject.GetComponent<PlayerController>();
        }

       

        // if (temp != null)
        // {

        //}
    }
}
