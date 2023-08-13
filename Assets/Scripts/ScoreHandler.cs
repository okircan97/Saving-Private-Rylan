using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    int highScore;
    public const string highScoreKey = "High Score";


    void Start(){
        scoreText.text = "SCORE: " + PlayerPrefs.GetInt(highScoreKey,0).ToString();
    }
}
