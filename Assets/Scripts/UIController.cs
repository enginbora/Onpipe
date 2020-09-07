using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI winScreenScore;
    [SerializeField] private TextMeshProUGUI loseScreenScore;
    [SerializeField] private TextMeshProUGUI completionPercentText;
    [SerializeField] private TextMeshProUGUI tapToPlay;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private Image fillBar;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject mainScreen;
    
    private float fillAmount;
    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        gameController.onScoreChanged.AddListener(UpdateScore);
        gameController.onGameStart.AddListener(DisableTapToPlay);
        gameController.multiplierChanged.AddListener(PopMultiplier);
        gameController.onScoreChanged.AddListener(FillTheBar);
        gameController.crossedFinishLine.AddListener(ActivateWinScreen);
        gameController.onFail.AddListener(ActivateLoseScreen);
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "0";
    }

    private void ActivateWinScreen()
    {
        mainScreen.SetActive(false);
        winScreen.SetActive(true);
        winScreenScore.text = gameController.score.ToString();
    }

    private void ActivateLoseScreen()
    {
        mainScreen.SetActive(false);
        loseScreen.SetActive(true);
        var completionText = Mathf.Round(fillBar.fillAmount * 10000) / 100;
        completionPercentText.text = "Completed " + completionText + "%";
        loseScreenScore.text = gameController.score.ToString();
    }

    private void FillTheBar()
    {
        if (fillAmount < 60)
        {
            fillAmount++;
            fillBar.fillAmount = fillAmount / 60;
            if (fillAmount >= 50)
            {
                arrow.SetActive(true);
            }
        }

        if (fillAmount >= 60)
        {
            arrow.SetActive(false);
        }
    }

    private void DisableTapToPlay()
    {
        tapToPlay.enabled = false;
    }

    private void PopMultiplier()
    {
        multiplierText.GetComponent<Animator>().SetTrigger("Pop");
        if (gameController.scoreMultiplier != 1)
        {
            multiplierText.text = "x" + gameController.scoreMultiplier.ToString();
        }
        else
        {
            multiplierText.text = "";
        }
    }

    private void UpdateScore()
    {
        scoreText.text = gameController.score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
