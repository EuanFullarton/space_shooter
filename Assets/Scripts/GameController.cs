using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{
	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public Text scoreText;
	public Text gameOverText;
	public Text highScoreText;
	public GameObject restartButton;

	private bool gameOver;
	private int score;
	private int highScore;

	void Start ()
	{
		gameOver = false;
		gameOverText.text = "";
		restartButton.SetActive (false);

		highScore = PlayerPrefs.GetInt ("High Score", 0);
		score = 0;
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
	}

	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (true)
		{
			for (int i = 0; i < hazardCount; i++)
			{
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);

			if (gameOver)
			{
				restartButton.SetActive (true);
				break;
			}
		}
	}
		
	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		if (score > highScore) 
		{
			highScore = score;
			PlayerPrefs.SetInt ("High Score", highScore);
		}
		UpdateScore ();
	}

	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
		highScoreText.text = "High Score: " + highScore;
	}

	public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
		PlayerPrefs.SetInt("High Score", highScore);
		PlayerPrefs.Save();
	}

	public void RestartGame () {
		Application.LoadLevel (Application.loadedLevel);
	}
}