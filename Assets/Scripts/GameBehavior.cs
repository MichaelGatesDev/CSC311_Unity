﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI itemsCollectedText;
    public TextMeshProUGUI statusText;
    private Coroutine statusTextRoutine;
    public GameObject CON_WinScreen;
    public GameObject CON_LoseScreen;
    public TextMeshProUGUI levelIndicatorText;

    private AudioHelper _audioHelper;

    public GameObject PlayerObj;
    public PlayerBehavior PlayerBehavior;

    public GameObject LevelChangeTrigger;

    public AudioClip BackgroundMusic;
    public AudioClip LevelCompleteSound;

    private void Awake()
    {
        CON_WinScreen.SetActive(false);
        CON_LoseScreen.SetActive(false);
        statusText.SetText("");
    }

    private void Start()
    {
        _audioHelper = GameObject.Find("Audio Manager").GetComponent<AudioHelper>();

        var bgMusicObj = new GameObject();
        var bgMusicAudio = bgMusicObj.gameObject.AddComponent<AudioSource>();
        bgMusicAudio.clip = BackgroundMusic;
        bgMusicAudio.volume = 0.25f;
        bgMusicAudio.loop = true;
        bgMusicAudio.Play();

        LevelChangeTrigger.SetActive(false);
        UpdateHealthText();
        UpdateItemsText();
        levelIndicatorText.SetText("Level " + int.Parse(SceneManager.GetActiveScene().name.Replace("Level", "")));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
            Application.Quit();
#endif
        }
    }

    public void UpdateHealthText()
    {
        healthText.SetText($"Player Health: {PlayerBehavior.lives}/{PlayerBehavior.maxLives}");
    }

    public void UpdateItemsText()
    {
        itemsCollectedText.SetText($"Items Collected: {PlayerBehavior.items}/{PlayerBehavior.maxItems}");
    }

    public void OnItemCollected()
    {
        var remainingItems = PlayerBehavior.maxItems - PlayerBehavior.items;

        UpdateItemsText();
        if (statusTextRoutine != null)
            StopCoroutine(statusTextRoutine);

        if (remainingItems > 0)
        {
            statusTextRoutine = StartCoroutine(TemporaryText(statusText, 3.0f, $"You collected another item! Only {remainingItems} remaining!"));
        }
        else
        {
            statusTextRoutine = StartCoroutine(TemporaryText(statusText, 3.0f, $"You collected all the items! Continue to the next level!"));
        }
    }

    public void OnLifeLost()
    {
        UpdateHealthText();
        if (statusTextRoutine != null)
            StopCoroutine(statusTextRoutine);

        if (PlayerBehavior.lives > 0)
        {
            statusTextRoutine = StartCoroutine(TemporaryText(statusText, 3.0f, $"You lost a life! Only {PlayerBehavior.lives} remaining!"));
        }
    }

    private static IEnumerator TemporaryText(TMP_Text textObj, float secondsToShow, string message)
    {
        textObj.SetText(message);
        yield return new WaitForSeconds(secondsToShow);
        textObj.SetText("");
    }

    public void OnGameWin()
    {
        healthText.gameObject.SetActive(false);
        itemsCollectedText.gameObject.SetActive(false);
        CON_WinScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnGameLose()
    {
        healthText.gameObject.SetActive(false);
        itemsCollectedText.gameObject.SetActive(false);
        CON_LoseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    public void LoadNextLevel()
    {
        //TODO playerprefs and score

        // level 1
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        //level 2
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            OnGameWin();
        }
    }

    public void OnLevelComplete()
    {
        LevelChangeTrigger.SetActive(true);
        _audioHelper.PlaySound(LevelCompleteSound, 1.0f, 1.0f);
    }
}