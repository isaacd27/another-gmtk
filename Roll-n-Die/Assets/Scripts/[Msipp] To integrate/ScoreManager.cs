using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public Text scoretext;
    int totalScore;
    float scoremulti;
    public float maxdietime;
    float dietime;

    public void setmultiply(float multi)
    {
        scoremulti = multi;
    }

    public void setScore(int score)
    {
        totalScore += Mathf.RoundToInt(score * scoremulti);
    }
    // Start is called before the first frame update
    void Start()
    {
        dietime = maxdietime;
    }

    // Update is called once per frame
    void Update()
    {
        scoretext.text = "Score: " + totalScore.ToString("0000");


    }

    public void resettime()
    {
        dietime = maxdietime;
    }

    public void OnHit()
    {
        scoremulti = scoremulti / 2;
    }

    private void FixedUpdate()
    {
        dietime -= Time.deltaTime;
        if(dietime <= 0)
        {
            scoremulti -= 1;
            //dietime = maxdietime
        }
    }
}
