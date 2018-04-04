using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required to switch scenes

public class LevelManager : MonoBehaviour {
    public static LevelManager levelManager;
    public int CurrentSceneIndex;
    public int score;
    // Use this for initialization
    void Awake()
    {
        if (levelManager == null)
        {
            DontDestroyOnLoad(gameObject);
            levelManager = this;
        }
        else if (levelManager != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(string name)
    {
        print("Loading " + name);
        SceneManager.LoadScene(name);
    }

    public void EndGame()
    {
        Debug.Log("Quit requested");
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.N))
        {
            LoadNextLevel();
        }
    }
    public void LoadStart()
    {
        SceneManager.LoadScene(0);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(FindObjectOfType<ScoreTracker>())FindObjectOfType<ScoreTracker>().IncreaseScore(score);
        if (FindObjectOfType<PlayerPlatformer2D>())FindObjectOfType<PlayerPlatformer2D>().score = score;
        print("Score: " + score);
        if(scene.buildIndex == 0)
        {
            score = 0;
        }
    }
    
}
