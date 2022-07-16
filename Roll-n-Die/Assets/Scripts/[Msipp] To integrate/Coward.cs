using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coward : BasicEnemy
{
    // Start is called before the first frame update
   
    public float cowardrad;
    public float rotationSpeed;
    //public ffloat maxattack;
    //float EnemyCool
    // Update is called once per frame

    public void Update()
    {
        Movement();
            //EnemyCool -= time.deltatime;
    }

    protected override void Movement()
    {


        float coward = Vector3.Distance(transform.position, Player.transform.position);

        if (coward <= cowardrad)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.position - Player.transform.position);
            float str = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
            transform.Translate(speed * Time.deltaTime, speed * Time.deltaTime, 0);
        }
        else
        {
            Quaternion targetRotation = Quaternion.LookRotation(Player.transform.position - transform.position);

            /*  
            if(EnemyCool <= 0f){

            Projectile temp = GameObject.Instantiate(projPrefab, new Vector3(this.transform.position.x + d.x, this.transform.position.y + d.y), this.transform.rotation);

              temp.transform.position = this.transform.position + this.transform.up * 0.4f * Mathf.Sign(this.transform.localScale.x);
              // temp.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);


              temp.setDirection(d);
            }*/


        }


    }
}
