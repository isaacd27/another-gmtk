using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    public bool paused;
    public void pauseflip()
    {
        paused = !paused;
    }
    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            transform.position += movement * Time.deltaTime * moveSpeed;
        }


    }

   public void death()
    {
        GameManager.Instance.GameOver();
    }
 
}
