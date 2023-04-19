using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour {
  public float health = 4;
  public float speed = 2;
  public float visionRange = 1;
  public bool isRight;
  public float distanceToStop = 0.8f;

  private bool isPlayerInFront;
  private Vector2 direction;

  public Transform frontRaycastPoint;
  public Transform backRaycastPoint;
  public Animator anim;

  private Rigidbody2D rb;

  void Start() {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();

    if (isRight) {
      transform.eulerAngles = new Vector2(0, 0);
      direction = Vector2.right;
    } else {
      transform.eulerAngles = new Vector2(0, 180);
      direction = Vector2.left;
    }
  }

  void Update() {

  }

  void FixedUpdate() {
    GetPlayer();
    if (isPlayerInFront) {
      OnMove();
    }
  }

  void GetPlayer() {
    RaycastHit2D frontHit = Physics2D.Raycast(frontRaycastPoint.position, direction, visionRange);
    if (frontHit.collider != null && frontHit.transform.CompareTag("Player")) {
      isPlayerInFront = true;

      float distance = Vector2.Distance(transform.position, frontHit.transform.position);
      if (distance <= distanceToStop) {
        isPlayerInFront = false;
        rb.velocity = Vector2.zero;

        anim.SetInteger("Transition", 2);
        frontHit.transform.GetComponent<Player>().OnHit();
      }
    } else {
      isPlayerInFront = false;
      rb.velocity = Vector2.zero;
      anim.SetInteger("Transition", 0);
    }

    RaycastHit2D backHit = Physics2D.Raycast(backRaycastPoint.position, -direction, visionRange);

    if (backHit.collider != null && backHit.transform.CompareTag("Player")) {
      isRight = !isRight;
      isPlayerInFront = true;
    }
  }

  void OnMove() {
    anim.SetInteger("Transition", 1);

    if (isRight) {
      transform.eulerAngles = new Vector2(0, 0);
      direction = Vector2.right;
      rb.velocity = new Vector2(speed, rb.velocity.y);
    } else {
      transform.eulerAngles = new Vector2(0, 180);
      direction = Vector2.left;
      rb.velocity = new Vector2(-speed, rb.velocity.y);
    }
  }

  public void OnHit() {
    anim.SetTrigger("isHitting");
    health--;

    if (health <= 0) {
      speed = 0;
      anim.SetTrigger("isDead");
      Destroy(gameObject, 0.5f);
    }
  }

  private void OnDrawGizmos() {
    Gizmos.DrawRay(frontRaycastPoint.position, direction * visionRange);
  }
}
