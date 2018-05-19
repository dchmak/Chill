/*
* Created by Daniel Mak
*/

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    [Header("Initial Statistics")]
    [Range(0f, 10f)] public float movementSpeed;
    [Range(0f, 10f)] public float freezingMovementSpeed;
    [Range(0f, 100f)] public float maxHealth;

    [Header("Freeze settings")]
    public GameObject freezeConePrefab;
    public Transform freezeGun;
    public GameObject freezeCirclePrefab;
    public float freezeCircleTime;

    [Header("UI elements")]
    public Image healthBar;
    public Canvas healthCanvas;

    private float health;
    private bool isFreezing;
    private Vector2 movement;
    private Rigidbody2D rb;
    private GameObject freezeCone;

    public void TakeDamage(float damage) {
        if (health > 0) health -= damage;
    }

	private void Start () {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }
	
	private void Update () {
        // get WASD movement
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // rotate to look at mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector2 dir = ((Vector2)ray.origin - (Vector2)transform.position).normalized;
        float angle = -Mathf.Atan2(-dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        // freeze gun
        if (Input.GetMouseButtonDown(0)) {
            //freezeCone = Instantiate(freezeConePrefab, transform.position, Quaternion.identity);
            freezeCone = Instantiate(freezeConePrefab);
            freezeCone.transform.SetParent(transform);
            freezeCone.transform.localPosition = Vector3.zero;
            freezeCone.transform.localRotation = Quaternion.Euler(0, 90, 90);
            freezeCone.transform.localScale = new Vector3(1, 1, 1);

            isFreezing = true;
        } // start freezing
        if (Input.GetMouseButtonUp(0)) {
            freezeCone.GetComponent<ParticleSystem>().Stop();
            freezeCone.transform.parent = null;
            freezeCone.transform.localScale = new Vector3(1, 1, 1);
            Destroy(freezeCone, 5f);
            isFreezing = false;
        } // end freezing

        // freeze cone
        if (Input.GetMouseButtonUp(1)) {
            //print((Vector2)ray.origin);
            GameObject freezeCircle = Instantiate(freezeCirclePrefab, (Vector2)ray.origin, Quaternion.identity);
            Destroy(freezeCircle, freezeCircleTime);
        } // spawn freeze circle

        // health UI
        healthBar.fillAmount = health / maxHealth;
        healthCanvas.transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate() {
        //print(movement);
        if (isFreezing) rb.velocity = movement * freezingMovementSpeed;
        else rb.velocity = movement * movementSpeed;
    }

    private void LateUpdate() {
        //print(health);
    }
}