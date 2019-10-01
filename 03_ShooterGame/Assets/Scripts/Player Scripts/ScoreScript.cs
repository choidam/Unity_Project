using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{


    public static int score;
    Text scoreText;
    
    private void Awake() {
       scoreText = GetComponent<Text>();
       score = 0;
    }

    public void Update(){
        scoreText.text = "Score : " + score;
    }
}
