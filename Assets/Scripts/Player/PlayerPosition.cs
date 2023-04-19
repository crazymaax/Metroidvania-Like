using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour {
  public static PlayerPosition instance;

  private Transform player;

  void Start() {
    instance = this;

    player = GameObject.FindGameObjectWithTag("Player").transform;
    Checkpoint();
  }

  void Update() {

  }

  public void Checkpoint() {
    Vector3 playerPosition = transform.position;
    playerPosition.z = 0f;

    player.position = playerPosition;
  }
}
