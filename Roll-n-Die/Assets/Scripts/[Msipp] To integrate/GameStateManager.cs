using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public int Bombs;

    public int lives = 6;

   // public  int StartFruits = 0;

   // public float collsize;

   // public float xsize = 1;

    //public Vector3 checkpointpos;

    public Text BombText;

     bool Firstbomb = false;

    public int getBombs()
    {
        return Bombs;
    }

    public void BombDia()
    {
        
        if (!Firstbomb)
        {
            BombText.gameObject.SetActive(true);
            BombText.transform.localScale = new Vector3(1, 1);
            Firstbomb = true;
            BombText.text = "Take This!";
            float i = 0;
            if (i >= 5f * Time.deltaTime)
            {
                BombText.gameObject.SetActive(false);
                BombText.transform.localScale = Vector3.zero;

            }
            else
            {
                i += Time.deltaTime;
            }
        }
    }

    
public int getlives()
    {
        return lives;
    }

  

   



    private static GameStateManager instance;

    public static GameStateManager Instance
    {
        get { return instance; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            GameObject.Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
     
    }

  
    public void ChangeLives(int DeltaChange)
    {
        lives += DeltaChange;
    }

    public void ChangeBombs(int DeltaChange)
    {
        Bombs += DeltaChange;
    }

   

    
    public void OnDeath()
    {
        Debug.Log("death trigger");
        ChangeLives(-1);

        if (lives < 0)
        {
            Debug.Log("no lives!");
            Bombs = 0;
            lives = 6;
            SceneManager.LoadScene("GameOver");
        }
    }
 
}
