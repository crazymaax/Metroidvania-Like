using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
  public AudioClip coinSound;
  public AudioClip jumpSound;
  public AudioClip onHitSound;

  private AudioSource audioSrc;

  void Start() {
    audioSrc = GetComponent<AudioSource>();
  }

  void Update() {

  }

  public void PlaySFX(AudioClip sfx) {
    audioSrc.PlayOneShot(sfx);
  }
}
