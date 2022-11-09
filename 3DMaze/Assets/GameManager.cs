using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text gameOverText;
    [SerializeField] PlayerController player;
    [SerializeField] AudioManager audioManager;
    [SerializeField] Hole hole;

    string[] score = { "Hole in One", "Eagle", "Birdie", "Par" };

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (hole.Entered && !gameOverPanel.activeInHierarchy)
        {
            audioManager.PlayWinSFX();
            gameOverPanel.SetActive(true);

            // gameOverText.text = 
            //     player.ShootCount == 1 ? "Hole in One!" :
            //     player.ShootCount == 2 ? "Eagle" :
            //     player.ShootCount <= 4 ? "Par" :
            //     "Bogey";

            gameOverText.text = player.ShootCount <= 4 ?
                                    score[player.ShootCount - 1] :
                                    "Bogey";
        }
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
