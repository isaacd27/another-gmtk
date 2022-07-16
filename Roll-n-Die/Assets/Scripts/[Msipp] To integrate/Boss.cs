using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{

    public Fireball fprefab;
    // Start is called before the first frame update
 

    [SerializeField] float LeftXpos;
    [SerializeField] float RightXpos;

    bool hitleft = false;

    public GameObject player;
    private float firecool;
    public float firereset = 10f;

    public float speed = 3f;

    public int hp = 6;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        firecool = firereset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
           if (hp > 0)
            {
                hp -= 1;
            }
            else
            {
                SceneManager.LoadScene("Outro");
                Destroy(this.gameObject);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        movement();
        attack();
        firecool -= Time.deltaTime;
    }

    public void attack()
    {
        if (this.transform.position.x - this.transform.localScale.x < player.transform.position.x && this.transform.position.x + this.transform.localScale.x > player.transform.position.x)
        {
            if (firecool <= 0)
            {
                Fireball temp = GameObject.Instantiate(fprefab, this.transform.position, Quaternion.identity);
                firecool = firereset;
            }

        }
    }

    public void movement()
    {
        if (hitleft == false)
        {
            if (this.transform.position.x > LeftXpos)
            {

               // transform.localScale = new Vector3((float)3.38921, transform.localScale.y);

                transform.position += new Vector3(-speed * Time.deltaTime, 0f, 0f);
                // Debug.Log("Left");
            }
            else if (this.transform.position.x <= LeftXpos)
            {
                hitleft = true;
            }
        }
        else
        {
            if (this.transform.position.x < RightXpos)
            {

               // transform.localScale = new Vector3((float)-3.38921, transform.localScale.y);

                transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);

                // Debug.Log("right");


            }
            else if (this.transform.position.x >= RightXpos)
            {
                hitleft = false;
            }
        }
    }
}
