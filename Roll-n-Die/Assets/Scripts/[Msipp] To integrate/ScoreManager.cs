using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public Text scoretext;
    int totalScore;
    float scoremulti;


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
        
    }

    // Update is called once per frame
    void Update()
    {
        scoretext.text = "Score: " + totalScore.ToString("0000");
    }
}
