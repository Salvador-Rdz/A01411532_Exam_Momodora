﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour {
    public Text scoreText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void IncreaseScore(int amount)
    {
        LevelManager lm = FindObjectOfType<LevelManager>();
        lm.score = amount;
        scoreText.text = "" + amount;
    }

}