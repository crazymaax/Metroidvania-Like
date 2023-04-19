using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
  public float speed = 5f;
  public float jumpForce = 10f;
  public float attackRadius = 3f;
  public TMP_Text scoreText;
  public GameObject gameOverPanel;
  public Button restartButton;

  private float recoveryTime;
  private bool isDead;
  private bool isJumping;
  private bool isAttacking;
  private bool doubleJump;

  public LayerMask enemyLayer;
  public Animator anim;
  public Transform hitPoint;

  public static Player instance;

  private Rigidbody2D rb;
  private PlayerAudio playerAudio;
  private PlayerHealth healthSystem;

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
    rb = GetComponent<Rigidbody2D>();
    playerAudio = GetComponent<PlayerAudio>();
    healthSystem = GetComponent<PlayerHealth>();
    restartButton.onClick.AddListener(GameController.instance.RestartGame);
  }

  void Update() {
    Jump();
    Attack();

    if (healthSystem.health <= 0) {
      StartCoroutine(ShowGameOverAfterDelay());
    }
  }

  void FixedUpdate() {
    Move();
  }

  void Move() {
    float movement = Input.GetAxis("Horizontal");
    rb.velocity = new Vector2(movement * speed, rb.velocity.y);

    if (movement > 0) {
      if (!isJumping && !isAttacking) {
        anim.SetInteger("Transition", 1);
      }
      transform.eulerAngles = new Vector3(0, 0, 0);
    } else if (movement < 0) {
      if (!isJumping && !isAttacking) {
        anim.SetInteger("Transition", 1);
      }
      transform.eulerAngles = new Vector3(0, 180, 0);
    } else if (movement == 0 && !isJumping && !isAttacking) {
      anim.SetInteger("Transition", 0);
    }
  }

  void Jump() {
    if (Input.GetButtonDown("Jump")) {
      if (!isJumping) {
        anim.SetInteger("Transition", 2);
        playerAudio.PlaySFX(playerAudio.jumpSound);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isJumping = true;
        doubleJump = true;
      } else if (doubleJump) {
        anim.SetInteger("Transition", 2);
        playerAudio.PlaySFX(playerAudio.jumpSound);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        doubleJump = false;
      }
    }
  }

  void Attack() {
    if (Input.GetButtonDown("Fire1")) {
      isAttacking = true;
      Collider2D hit = Physics2D.OverlapCircle(hitPoint.position, attackRadius, enemyLayer);
      anim.SetInteger("Transition", 3);
      playerAudio.PlaySFX(playerAudio.onHitSound);
      StartCoroutine(OnAttack());

      if (hit != null) {

        if (hit.GetComponent<Slime>()) {
          hit.GetComponent<Slime>().OnHit();
        }

        if (hit.GetComponent<Goblin>()) {
          hit.GetComponent<Goblin>().OnHit();
        }
      }
    }
  }

  public void OnHit() {
    recoveryTime += Time.deltaTime;

    if (!isDead) {
      anim.SetTrigger("isHitting");
      healthSystem.health--;

      if (healthSystem.health <= 0) {
        isDead = true;
        anim.SetTrigger("isDead");
      } else {
        StartCoroutine(Recover());
      }
    }
  }

  void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.layer == 3 || collision.gameObject.layer == 10) {
      isJumping = false;
    }

    if (collision.gameObject.layer == 9) {
      PlayerPosition.instance.Checkpoint();
      healthSystem.health--;
    }
  }

  void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject.layer == 6) {
      OnHit();
    }

    if (collision.CompareTag("Coin")) {
      playerAudio.PlaySFX(playerAudio.coinSound);
      collision.GetComponent<Animator>().SetTrigger("isHitting");
      GameController.instance.GetCoin();
      Destroy(collision.gameObject, 0.3f);
    }
  }

  void OnDrawGizmos() {
    Gizmos.DrawWireSphere(hitPoint.position, attackRadius);
  }

  IEnumerator Recover() {
    isDead = true;
    yield return new WaitForSeconds(0.7f);
    isDead = false;
  }

  IEnumerator ShowGameOverAfterDelay() {
    yield return new WaitForSeconds(0.7f);
    GameController.instance.ShowGameOver();
  }

  IEnumerator OnAttack() {
    yield return new WaitForSeconds(0.333f);
    isAttacking = false;
  }
}
