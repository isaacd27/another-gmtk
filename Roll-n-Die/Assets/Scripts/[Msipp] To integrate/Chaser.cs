using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : BasicEnemy
{
   // public int hp = 1;
    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<PlayerController>().gameObject;
        Debug.Assert(Player);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    protected override void Movement()
    {


        if (this.transform.position.x > Player.transform.position.x)
        {
            //transform.localScale = new Vector3((float)-1.943782, transform.localScale.y);
            transform.position += new Vector3(-speed * Time.deltaTime, 0f);
        }
        else if (this.transform.position.x < Player.transform.position.x)
        {
            //transform.localScale = new Vector3((float)1.943782, transform.localScale.y);
            transform.position += new Vector3(speed * Time.deltaTime, 0f);
        }

        if (this.transform.position.y > Player.transform.position.y)
        {
            transform.position += new Vector3(0f, -speed * Time.deltaTime);
        }
        else if (transform.position.y < Player.transform.position.y)
        {
            transform.position += new Vector3(0f, speed * Time.deltaTime);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Change this for check type instead + set collision 2d matrix layer 
        //if (collision.gameObject.CompareTag("Projectile"))
        //    hp -= 1;
        //        if(hp <= 0)
        //{
        //    Destroy(this.gameObject);
        //}
    }
}





