using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GUIText scoreText;
	public GUIText waveText;
	public GUIText replayText;
	public GUIText killStreakText;
	public GUIText scoreCalcText;
	public GUIText totalKillText;
	public GUIText currentKills;
	public GUIText quitText;

	public CurvedText killStreakUI;
	public CurvedText scoreUI;
	public CurvedText killTimer;

	public int score;
	public int waveNumber;
	public int totalWaves;
	public float waveTextDisplayTime;
	public bool isWin;
	public bool gameOver;

	public float setScoreTimer;
	private float scoreTimer;
	private int totalKills;
	private int currentScore;
	private int scoreMultiplier;
	private int currentKillStreak;
	private bool changeWaves;

	private EnemySpawnController enemySpawnController;

	void Start () {
		enemySpawnController = GameObject.FindWithTag ("EnemyStop").GetComponent<EnemySpawnController> ();
		killStreakUI = GameObject.FindWithTag ("KillstreakUI").GetComponent<CurvedText> ();
		killTimer = GameObject.FindWithTag ("TimerUI").GetComponent<CurvedText> ();
		scoreUI = GameObject.FindWithTag ("ScoreUI").GetComponent<CurvedText> ();
		changeWaves = false;
		gameOver = false;
		isWin = false;
		ResetGUI ();
		waveNumber = 1;
		totalKills = 0;
		killStreakText.text = "Killstreak Time Remaining: " + scoreTimer;
		scoreCalcText.text = "Score += (" + currentScore + ") x" + scoreMultiplier;

		//Reticle info
		killTimer.text = "Time: " + Mathf.Round ((scoreTimer * 100f)) / 100f;
		scoreUI.text = "Score += (" + currentScore + ") x" + scoreMultiplier;
		killStreakUI.text = "Killstreak: " + currentKillStreak;
	}


	void Update () {
		totalKillText.text = "Total Kills: " + totalKills;
		currentKills.text = "Killstreak: " + currentKillStreak;
		killStreakUI.text = "Killstreak: " + currentKillStreak;
		if (!enemySpawnController.CheckEndWave()) {
			killStreakText.text = "Killstreak Time Remaining: " + scoreTimer;
			killTimer.text = "Time: " + Mathf.Round ((scoreTimer * 100f)) / 100f;
			scoreCalcText.text = "Score += (" + currentScore + ") x" + scoreMultiplier;
			scoreUI.text = "Score += (" + currentScore + ") x" + scoreMultiplier;
			scoreTimer -= Time.deltaTime;
		}

		if (scoreTimer <= 0) {
			ResetGUI ();
		}

		if (isWin) {
			ResetGUI ();
			ResetKillAndScoreText ();
			scoreTimer -= Time.deltaTime;
			if (Input.GetKeyDown (KeyCode.R)) {
				Application.LoadLevel (Application.loadedLevel);
			}
		}

		if (gameOver) {
			ResetGUI ();
			ResetKillAndScoreText ();
			scoreTimer -= Time.deltaTime;
			if (Input.GetKeyDown (KeyCode.R)) {
				Application.LoadLevel (Application.loadedLevel);
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			ResetGUI ();
			ResetKillAndScoreText ();
			scoreTimer -= Time.deltaTime;
			Application.LoadLevel (0);
		}


	}

	public void UpdateWaveText(string newWaveText) {
		waveText.text = newWaveText;
		if (waveText.text == "Wave 0 Incoming") {
			waveText.text = "Wave 1 Incoming";
		}
		if (waveText.text == "Wave 0 Complete") {
			waveText.text = "wave 1 Complete";
		}
		StartCoroutine (ShowWaveText ());
	}

	public void AddScore(int newScoreValue) {
		if (currentKillStreak == 0) {
			scoreTimer = setScoreTimer;
		} else {
			scoreTimer += 1;
		}
		currentKillStreak += 1;
		totalKills += 1;
		if (currentKillStreak % 10 == 0) {
			scoreMultiplier += 1;
		}
		currentScore += newScoreValue;
	}

	public void SetIsWin() {
		replayText.gameObject.SetActive (true);
		quitText.gameObject.SetActive (true);
		waveText.text = "You Win!";
		waveText.gameObject.SetActive (true);
		isWin = true;
	}

	public void GameOver() {
		waveText.text = "Game Over!";
		replayText.gameObject.SetActive (true);
		quitText.gameObject.SetActive (true);
		waveText.gameObject.SetActive (true);
		gameOver = true;
	}

	void ResetGUI() {
		score += (currentScore * scoreMultiplier);
		UpdateScore ();
		scoreTimer = 0;
		currentScore = 0;
		scoreMultiplier = 1;
		currentKillStreak = 0;
	}

	void ResetKillAndScoreText() {
		killStreakText.text = "Killstreak Time Remaining: " + scoreTimer;
		scoreCalcText.text = "Score += (" + currentScore + ") x" + scoreMultiplier;
		killTimer.text = "Time: " + Mathf.Round ((scoreTimer * 100f)) / 100f;
		scoreUI.text = "Score += (" + currentScore + ") x" + scoreMultiplier;

	}

	void UpdateScore() {
		scoreText.text = "Score: " + score;
	}

	IEnumerator ShowWaveText() {
		waveText.gameObject.SetActive (true);
		quitText.gameObject.SetActive (true);
		yield return new WaitForSeconds (waveTextDisplayTime);
		changeWaves = false;
		if (!gameOver && !isWin) {
			waveText.gameObject.SetActive (false);
			quitText.gameObject.SetActive (false);
		}
	}
		
}
