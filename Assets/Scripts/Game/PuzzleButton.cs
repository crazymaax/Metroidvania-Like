using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour {
  public Animator barrierAnim;
  public LayerMask layer;

  public float radius;

  private bool isPressed;
  private Animator anim;

  void Start() {
    anim = GetComponent<Animator>();
  }

  void FixedUpdate() {
    OnCollision();
  }

  void OnPress() {
    anim.SetBool("isPressed", true);
    barrierAnim.SetBool("isPressed", true);
  }

  void OnExit() {
    anim.SetBool("isPressed", false);
    barrierAnim.SetBool("isPressed", false);
  }

  void OnCollision() {
    Collider2D hit = Physics2D.OverlapCircle(transform.position, radius, layer);

    if (hit != null) {
      OnPress();
      hit = null;
    } else {
      OnExit();
    }
  }

  void OnDrawGizmos() {
    Gizmos.DrawWireSphere(transform.position, radius);
  }
}
