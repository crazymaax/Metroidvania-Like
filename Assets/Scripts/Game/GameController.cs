using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour {
  public static GameController instance;

  public int score;

  void Awake() {
    if (instance == null) {
      instance = this;
      DontDestroyOnLoad(this);
    } else if (instance != this) {
      Destroy(instance.gameObject);
      instance = this;
      DontDestroyOnLoad(this);
    }
  }

  void Start() {
    Time.timeScale = 1;

    if (PlayerPrefs.GetInt("Score") > 0) {
      score = PlayerPrefs.GetInt("Score");
      Player.instance.scoreText.text = score.ToString();
    }
  }

  void Update() {
    if (Input.GetButtonDown("Cancel") && Time.deltaTime != 0) {
      RestartGame();
    }
  }

  public void GetCoin() {
    score++;
    Player.instance.scoreText.text = score.ToString();

    PlayerPrefs.SetInt("Score", score);
  }

  public void ShowGameOver() {
    Time.timeScale = 0;
    Player.instance.gameOverPanel.SetActive(true);
  }

  public void RestartGame() {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
