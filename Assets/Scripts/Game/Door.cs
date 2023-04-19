using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {
  public int levelIndex;
  void Start() {

  }

  void Update() {

  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      SceneManager.LoadScene(levelIndex);
    }
  }
}
