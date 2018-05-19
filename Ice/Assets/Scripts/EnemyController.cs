/*
* Created by Daniel Mak
*/

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour {

    [Header("Initial Statistics")]
    [Range(0f, 10f)] public float speed;
    [Range(0f, 1000f)] public float angularSpeed;
    [Range(0f, 100f)] public float damage;
    [Range(0f, 100f)] public float maxHealth;
    [Range(0f, 10f)] public float invulnerableTime;

    private float health;
    private float invulnerableCount;
    private Transform target;
    private Rigidbody2D rb;
    private AudioController audioController;

    public void TakeDamage(float damage) {
        if (invulnerableCount <= 0) {
            health -= damage;
            invulnerableCount = invulnerableTime;
        }
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) {
            print("Nothing to follow");
        } else { 
            target = GetClosestObject(players);
        }

        audioController = FindObjectOfType<AudioController>();
    }

    private void Update() {
        if (invulnerableCount > 0) invulnerableCount -= Time.deltaTime;
    }

    private void FixedUpdate () {
        if (target != null) {
            Vector2 dir = ((Vector2)target.position - rb.position).normalized;

            float rotateAmount = Vector3.Cross(transform.right, dir).z;

            rb.velocity = transform.right * speed;
            rb.angularVelocity = rotateAmount * angularSpeed;
        }
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(damage);
        }
    }

    Transform GetClosestObject(GameObject[] obj) {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in obj) {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        return bestTarget;
    }
}
