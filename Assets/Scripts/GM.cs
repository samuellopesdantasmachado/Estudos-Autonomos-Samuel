﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {

	public static GM instance = null;

    public float yMinLive = -10f;
	public Transform spawnPoint;
	public GameObject playerPrefab;

	public float maxTime = 120f;
	bool timerOn = true;
	float timeLeft;

	public UI ui;

	GameData data = new GameData();


	PlayerCtrl player;

	public float timeToRespawn = 2f;

	void Awake(){
		if (instance == null){
			instance = this;
		}
	}

	public void RespawnPlayer(){
		Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
	}

	void Start () {
		if (player == null){
			RespawnPlayer();
		}
		timeLeft = maxTime;
	}
	
	void Update () {
		if (player == null){
			GameObject obj = GameObject.FindGameObjectWithTag("Player");
			if (obj != null){
				player = obj.GetComponent<PlayerCtrl>();
			}
		}
		UpdateTimer();
		DisplayHudData();
	}

	public void RestartLevel(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void ExitToMainMenu(){
		LoadScene("MainMenu");
	}

	public void LoadScene(string sceneName){
		SceneManager.LoadScene(sceneName);
	}

	public void CloseApp(){
		Application.Quit();
	}

	void UpdateTimer(){
		if (timerOn){
			timeLeft = timeLeft - Time.deltaTime;
			if (timeLeft <= 0f){
				timeLeft = 0;
				ExpirePlayer();
			}
		}
	}

	void DisplayHudData(){
		ui.hud.txtCoinCount.text = "x " + data.coinCount;
		ui.hud.txtLifeCount.text = "x " + data.lifeCount;
		ui.hud.txtTimer.text = "Timer: " + timeLeft.ToString("F1");
	}

	public void IncrementCoinCount(){
		data.coinCount++;
	}

	public void DecrementLives(){
		data.lifeCount--;
	}

	public void KillPlayer(){
		if (player != null){
			Destroy(player.gameObject);
			DecrementLives();
			if (data.lifeCount > 0){
			Invoke("RespawnPlayer", timeToRespawn);
			}
			else {
				GameOver();
			}
		}
	}

	public void ExpirePlayer(){
		if (player != null){
			Destroy(player.gameObject);
		}
		GameOver();
	}

	void GameOver(){
		timerOn = false;
		ui.gameOver.txtCoinCount.text = "Coins: " + data.coinCount;
		ui.gameOver.txtTimer.text = "Timer: " + timeLeft.ToString("F1");
		ui.gameOver.gameOverPanel.SetActive(true);

	}

	public void LevelComplete(){
		Destroy(player.gameObject);
		timerOn = false;
		ui.gameOver.txtCoinCount.text = "Coins: " + data.coinCount;
		ui.gameOver.txtTimer.text = "Timer: " + timeLeft.ToString("F1");
		ui.gameOver.gameOverPanel.SetActive(true);
	}

    
}