/*
* Created by Daniel Mak
*/

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    [Range(0f, 10f)] public float movementSpeed;
    [Range(0f, 100f)] public float maxHealth;
    [Range(0f, 10f)] public float invulnerableTime;

    public GameObject freezeConePrefab;

    public GameObject freezeCirclePrefab;
    public float freezeTime;

    private float health;
    private float invulnerableCount;
    private Vector2 movement;
    private Rigidbody2D rb;
    private GameObject freezeCone;

    public void TakeDamage(float damage) {
        if (invulnerableCount <= 0f) {
            health -= damage;
            invulnerableCount = invulnerableTime;
        }
    }

	private void Start () {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
	}
	
	private void Update () {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetMouseButtonDown(0)) {
            freezeCone = Instantiate(freezeConePrefab, transform.position, Quaternion.identity);
        }
        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 dir = ((Vector2)ray.origin - (Vector2)transform.position).normalized;
            float angle = -Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            freezeCone.transform.rotation = Quaternion.Euler(angle, 90, 90);
        }
        if (Input.GetMouseButtonUp(0)) {
            Destroy(freezeCone);
        }

        if (Input.GetMouseButtonUp(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //print((Vector2)ray.origin);
            GameObject freezeCircle = Instantiate(freezeCirclePrefab, (Vector2)ray.origin, Quaternion.identity);
            Destroy(freezeCircle, freezeTime);
        }

        if (invulnerableCount > 0) invulnerableCount -= Time.deltaTime;
    }

    private void FixedUpdate() {
        //print(movement);
        rb.velocity = movement * movementSpeed;
    }

    private void LateUpdate() {
        //print(health);
    }
}