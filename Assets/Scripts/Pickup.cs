using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public int heal;
    [Range(100,1000)]
    public int score;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public int getHeal()
    {
        return heal;
    }
    public int getScore()
    {
        return score;
    }
}
