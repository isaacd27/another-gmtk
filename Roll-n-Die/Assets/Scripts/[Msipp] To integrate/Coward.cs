using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coward : BasicEnemy
{
    // Start is called before the first frame update
   
    public float cowardrad;
    public float rotationSpeed;
    //change this to whatever you want to name the enemy projectile.
    public EnemyShot projPrefab;
    
    public float maxattack = 1;
    float EnemyCool;
    // Update is called once per frame

    public void Update()
    {
        Movement();
        EnemyCool -= Time.deltaTime;
    }

    protected override void Movement()
    {
        float coward = Vector3.Distance(transform.position, Player.transform.position);

        if (coward <= cowardrad)
        {
            Vector2 targetDirection = transform.position - Player.transform.position;
            Vector2 movement = targetDirection * speed;
            Vector2 newPos = GetComponent<Rigidbody2D>().position + movement * Time.fixedDeltaTime;
            transform.position = newPos;
        }
        else
        {
            // lookat the player

           // transform.LookAt(Player.transform.position);
            if (EnemyCool <= 0f)
            {

                EnemyShot temp = GameObject.Instantiate(projPrefab, new Vector3(this.transform.position.x, this.transform.position.y), this.transform.rotation);

             temp.transform.position = this.transform.position + this.transform.up * 0.4f * Mathf.Sign(this.transform.localScale.x);
                temp.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);


                temp.setDirection(Player.transform.position - transform.position);

                EnemyCool = maxattack;
            }
            else
            {
                Vector2 targetDirection =   Player.transform.position - transform.position;
                Vector2 movement = targetDirection * speed;
                Vector2 newPos = GetComponent<Rigidbody2D>().position + movement * Time.fixedDeltaTime;
                transform.position = newPos;
            }


        }




    }


}
