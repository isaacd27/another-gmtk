using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(0f, -3f, 0f);

        transform.position += movement * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // temp = collision.gameObject.GetComponent<Zombie>();
      
            Boss temp = collision.gameObject.GetComponent<Boss>();
            if (temp == null)
            {
                Destroy(this.gameObject);
                Debug.Log(collision.gameObject);
            }


        

        // {

        //}
    }
}
