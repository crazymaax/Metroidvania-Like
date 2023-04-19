using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
  public int health = 5;
  public int healthCount = 5;
  public Image[] hearts;
  public Sprite heartFullSprite;
  public Sprite heartEmptySprite;

  void Start() {

  }

  void Update() {
    for (int i = 0; i < hearts.Length; i++) {
      if (i < health) {
        hearts[i].sprite = heartFullSprite;
      } else {
        hearts[i].sprite = heartEmptySprite;
      }

      if (i < healthCount) {
        hearts[i].enabled = true;
      } else {
        hearts[i].enabled = false;
      }
    }
  }

}
