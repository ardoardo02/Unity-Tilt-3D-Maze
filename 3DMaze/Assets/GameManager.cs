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
    [Header("Game Settings")]
    [SerializeField] int WinHole = 1;
    int WinHoleCount;
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

    private void OnHoleEntered(Collider obj, bool isWin)
    {
        Debug.Log(isWin ? "Menang" : "Kalah");

        audioManager.PlayHoleEnterSFX(isWin);
        WinHoleCount++;

        if(WinHole != WinHoleCount && isWin)
            return;

        gameOverPanel.SetActive(true);

        var cameraController = Camera.main.GetComponent<CameraTouchController>();
        if(cameraController != null) cameraController.enabled = false;

        if(!isWin){
            gameOverText.text = "Level Failed";
            nextButton.SetActive(false);
            return;
        }

        // Menang
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
