using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [HideInInspector] public UnityEvent onPipeExit;    //when the player goes through and exits a pipe (Cylinder)
    [HideInInspector] public UnityEvent onLevelComplete;    //when the player is eligible to finish the level, not actually crossed the finish line.
    [HideInInspector] public UnityEvent crossedFinishLine;
    [HideInInspector] public UnityEvent onGameStart;
    [HideInInspector] public UnityEvent onObjectCreated;    //when a new object is instantiated in the scene
    [HideInInspector] public UnityEvent onScoreChanged;    
    [HideInInspector] public UnityEvent onFail;
    [FormerlySerializedAs("multiplierIncreased")] [HideInInspector] public UnityEvent multiplierChanged;
    [HideInInspector] public UnityEvent comboBreaker;
    
    public int score { get; private set; }
    public int scoreMultiplier { get; private set;} = 1;
    private int feverCalculator = 0;
    private int levelProgress = 0;         //we complete the level when this is 60
    [HideInInspector] public bool gameStarted;
    private bool canRestart = false;  //activates during the win/lose screens
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            StartTheGame();
        }

        if (canRestart && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void FinishLevel()
    {
        onLevelComplete.Invoke();
    }

    public void CrossedFinishLine()
    {
        crossedFinishLine.Invoke();
        canRestart = true;
    }

    public void Failed()
    {
        onFail.Invoke();
        canRestart = true;
    }

    public void OnFever()
    {
        feverCalculator += 1;
        
        if (feverCalculator >= 18 && scoreMultiplier < 5)
        {
            IncreaseScoreMultiplier();
            feverCalculator = 0;
        }
    }

    public void ComboBreaker()
    {
        comboBreaker.Invoke();
        multiplierChanged.Invoke();
        scoreMultiplier = 1;
        feverCalculator = 0;
    }

    public void NewObjectCreated()
    {
        onObjectCreated.Invoke();
    }
    
    public void OnPipeExit()
    {
        onPipeExit.Invoke();
    }

    public void AddScore(int amount)
    {
        score += amount * scoreMultiplier;
        levelProgress++;
        onScoreChanged.Invoke();
        
        if (levelProgress >= 60)
        {
            FinishLevel();
        }
    }

    private void IncreaseScoreMultiplier()
    {
        scoreMultiplier++;
        multiplierChanged.Invoke();
    }

    private void StartTheGame()
    {
        onGameStart.Invoke();
        gameStarted = true;
    }
}
