using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {
  public float speed;
  public int health = 3;
  public float radius = 0.08f;
  public Transform hitPoint;
  public LayerMask layer;

  private Rigidbody2D rb;
  private Animator anim;

  void Start() {
    rb = GetComponent<Rigidbody2D>();
  }

  void FixedUpdate() {
    rb.velocity = new Vector2(speed, rb.velocity.y);
    anim = GetComponent<Animator>();
    OnCollision();
  }

  void OnCollision() {
    Collider2D hit = Physics2D.OverlapCircle(hitPoint.position, radius, layer);

    if (hit != null) {
      speed = -speed;
      if (transform.eulerAngles.y == 0) {
        transform.eulerAngles = new Vector3(0, 180, 0);
      } else {
        transform.eulerAngles = new Vector3(0, 0, 0);
      }

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

  void OnDrawGizmos() {
    Gizmos.DrawWireSphere(hitPoint.position, radius);
  }

}
