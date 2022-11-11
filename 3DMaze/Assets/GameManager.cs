using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject nextButton;
    [SerializeField] TMP_Text gameOverText;
    // [SerializeField] PlayerController player;
    [SerializeField] AudioManager audioManager;
    [SerializeField] Hole[] hole;

    // string[] score = { "Hole in One", "Eagle", "Birdie", "Par" };

    private void Start()
    {
        gameOverPanel.SetActive(false);

        foreach (var item in hole)
        {
            item.HoleEnterEvent += OnHoleEntered;
        }
    }

    private void OnDestroy() {
        foreach (var item in hole)
        {
            item.HoleEnterEvent -= OnHoleEntered;
        }
    }

    private void Update()
    {
        if(gameObject.activeInHierarchy) return;

        if (!gameOverPanel.activeInHierarchy)
        {
            audioManager.PlayWinSFX();
            gameOverPanel.SetActive(true);

            // gameOverText.text = 
            //     player.ShootCount == 1 ? "Hole in One!" :
            //     player.ShootCount == 2 ? "Eagle" :
            //     player.ShootCount <= 4 ? "Par" :
            //     "Bogey";

            // gameOverText.text = player.ShootCount <= 4 ?
            //                         score[player.ShootCount - 1] :
            //                         "Bogey";

            string currentSceneName = SceneManager.GetActiveScene().name;
            var currentLevel = int.Parse(currentSceneName.Split("Level")[1]);
            gameOverText.text = $"Level {currentLevel} Completed!";
        }
    }

    private void OnHoleEntered(Collider obj, Boolean isWin)
    {
        Debug.Log(isWin ? "Menang" : "Kalah");

        gameOverPanel.SetActive(true);

        if(!isWin){
            gameOverText.text = "Level Failed";
            nextButton.SetActive(false);
            return;
        }

        // Menang
        audioManager.PlayWinSFX();

        string currentSceneName = SceneManager.GetActiveScene().name;
        var currentLevel = int.Parse(currentSceneName.Split("Level")[1]);
        gameOverText.text = $"Level {currentLevel} Completed!";
    }

    public void BackToMainMenu()
    {
        SceneLoader.Load("MainMenu");
    }

    public void Replay()
    {
        SceneLoader.ReloadLevel();
    }

    public void PlayNext()
    {
        SceneLoader.LoadNextLevel();
    }
}
