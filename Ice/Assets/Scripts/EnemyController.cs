/*
* Created by Daniel Mak
*/

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class EnemyController : MonoBehaviour {

    [Header("Initial Statistics")]
    [Range(0f, 10f)] public float initialSpeed;
    [Range(0f, 1000f)] public float initialAngularSpeed;
    [Range(0f, 100f)] public float initialDamage;
    [Range(0f, 100f)] public float initialMaxHealth;
    [Range(0f, 5f)] public float initialTimeToMine;
    [Range(0f, 100f)] public float score;
    public float spawnMinRadius, spawnMaxRadius;
    [Range(0f, 100f)] public float recoverTime;

    [Header("Difficulty settings")]
    [Range(0f, 1000f)] public float difficultyInterval;
    [Range(1f, 2f)] public float difficultyScaler;

    [Header("UI elements")]
    public Image healthBar;
    public Canvas healthCanvas;
    public Image freezeImage;
    public Sprite[] freezeSprites;
    public GameObject mineBarObject;
    public Image mineBar;

    private bool isBeingMined;
    private int freezeStage;
    private float speed, angularSpeed, damage, maxHealth, timeToMine;
    private float health;
    private float minedTime;
    private float recoverTimer;
    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private GameManager gm;
    private GameObject player;

    public void TakeDamage(float damage) {
        if (health > 0) {
            health -= damage;
            //print(health);

            freezeStage = Mathf.FloorToInt(Mathf.Lerp(0, freezeSprites.Length - 1, (maxHealth - health) / maxHealth));
            if (freezeStage == freezeSprites.Length - 1) {
                gm.IncreaseScore(score);
                recoverTimer = recoverTime;
            }
        }
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        gm = FindObjectOfType<GameManager>();

        ResetStats();
        Relocate();
    }

    private void Update() {
        healthBar.fillAmount = health / maxHealth;
        healthCanvas.transform.rotation = Quaternion.identity;

        freezeImage.transform.localRotation = transform.rotation;

        mineBarObject.SetActive(isBeingMined);

        freezeImage.sprite = freezeSprites[freezeStage];

        //print(isBeingMined);

        // recover
        if (recoverTimer > 0 && freezeStage == freezeSprites.Length - 1) {
            recoverTimer -= Time.deltaTime;
            float a = Mathf.Lerp(0, 1, recoverTimer / recoverTime);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);
        }
    }

    private void FixedUpdate () {
        if (target != null) {
            Vector2 dir = ((Vector2)target.position - rb.position).normalized;

            float rotateAmount = Vector3.Cross(transform.right, dir).z;

            rb.velocity = transform.right * (speed * (freezeSprites.Length - freezeStage) / freezeSprites.Length);
            rb.angularVelocity = rotateAmount * (angularSpeed * (freezeSprites.Length - freezeStage) / freezeSprites.Length);
        }
	}

    private void LateUpdate() {
        if (recoverTimer <= 0 && freezeStage == freezeSprites.Length - 1) {
            ResetStats();
            Relocate();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (freezeStage < freezeSprites.Length - 1) {
                player.TakeDamage(damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (freezeStage >= freezeSprites.Length - 1) {
                StartMining(collision);
            }
        }
    } // Something entered mine range

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (isBeingMined) {
                minedTime += Time.deltaTime;
                mineBar.fillAmount = minedTime / timeToMine;
                //print(minedTime);
            } else {
                if (freezeStage >= freezeSprites.Length - 1) {
                    StartMining(collision);
                }
            }

            if (minedTime >= timeToMine) {
                MineSuccess(collision);
            }
        }
    } // Mining...

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && isBeingMined) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.SetIsMining(false);
            isBeingMined = false;

            if (minedTime >= timeToMine) {
                MineSuccess(collision);
            } else {
                MineFail();
            }

            ResetStats();
        }
    } // If leave too early, mining has failed. Else mining is successful.

    private void StartMining(Collider2D collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        player.SetIsMining(true);
        mineBarObject.SetActive(true);
        isBeingMined = true;
    }

    private void MineSuccess(Collider2D collision) {
        gm.IncreaseScore(score);

        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        player.Recharge();

        Relocate();
    }

    private void MineFail() {
        isBeingMined = false;

        maxHealth /= 2;
        //Relocate();
    }

    private void Relocate() {
        //gameObject.SetActive(false);

        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector2 pos = dir * Random.Range(spawnMinRadius, spawnMaxRadius);

        transform.position = pos + (Vector2)gm.player.transform.position;

        //gameObject.SetActive(true);
    }

    private void ResetStats() {
        freezeStage = 0;

        float scaler = Mathf.Pow(difficultyScaler, Mathf.FloorToInt(gm.GetTimeElapsed() / difficultyInterval));
        speed = initialSpeed * scaler;
        angularSpeed = initialAngularSpeed * scaler;
        damage = initialDamage * scaler;
        maxHealth = initialMaxHealth * scaler;
        timeToMine = initialTimeToMine * scaler;

        health = maxHealth;
        minedTime = 0;

        target = gm.player.transform;
    }
}
