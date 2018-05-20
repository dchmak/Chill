/*
* Created by Daniel Mak
*/

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour {

    [Header("Initial Statistics")]
    [Range(0f, 10f)] public float speed;
    [Range(0f, 1000f)] public float angularSpeed;
    [Range(0f, 100f)] public float damage;
    [Range(0f, 100f)] public float maxHealth;

    [Header("UI elements")]
    public Image healthBar;
    public Canvas healthCanvas;
    public Image freezeImage;
    public Sprite[] freezeSprites;

    private int freezeStage;
    private float health;
    private Transform target;
    private Rigidbody2D rb;
    private AudioController audioController;

    public void TakeDamage(float damage) {
        if (health > 0) health -= damage;
        //print(health);

        freezeStage = Mathf.FloorToInt(Mathf.Lerp(0, freezeSprites.Length - 1, (maxHealth - health) / maxHealth));
        //freezeStage = Mathf.FloorToInt((maxHealth - health) / maxHealth * freezeSprites.Length);
        freezeImage.sprite = freezeSprites[freezeStage];
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;

        target = GameObject.FindGameObjectWithTag("Player").transform;

        audioController = FindObjectOfType<AudioController>();
    }

    private void Update() {
        healthBar.fillAmount = health / maxHealth;
        healthCanvas.transform.rotation = Quaternion.identity;

        freezeImage.transform.localRotation = transform.rotation;
    }

    private void FixedUpdate () {
        if (target != null) {
            Vector2 dir = ((Vector2)target.position - rb.position).normalized;

            float rotateAmount = Vector3.Cross(transform.right, dir).z;

            rb.velocity = transform.right * (speed * (freezeSprites.Length - freezeStage) / freezeSprites.Length);
            rb.angularVelocity = rotateAmount * (angularSpeed * (freezeSprites.Length - freezeStage) / freezeSprites.Length);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (freezeStage != freezeSprites.Length - 1) {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.TakeDamage(damage);
            }
        }
    }
}
