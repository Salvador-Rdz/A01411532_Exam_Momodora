using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour {
    public int enemies = 1;
    private Text enemyCounter;
	// Use this for initialization
	void Start () {
        if (gameObject.GetComponent<Text>()) enemyCounter = gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.GetComponent<Text>())
        {
            enemyCounter.text = "" + enemies;
        }
        if (enemies == 0)
        {
            StartCoroutine(FreezeHit());
            StartCoroutine(Victory());
        }
	}

    public void enemyDied()
    {
        enemies--;
    }
    IEnumerator FreezeHit()
    {
        Time.timeScale = 0.2f;
        float pauseEndTime = Time.realtimeSinceStartup + 2f;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1;
    }
    IEnumerator Victory()
    {
        yield return new WaitForSeconds(8f);
        LevelManager lm = FindObjectOfType<LevelManager>();
        lm.LoadNextLevel();
    }
}
