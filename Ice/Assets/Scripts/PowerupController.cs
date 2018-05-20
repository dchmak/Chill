/*
* Created by Daniel Mak
*/

using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PowerupController : MonoBehaviour {

    public float minRadius, maxRadius;
    [Range(0f, 100f)] public float power;
    [Range(0f, 100f)] public float score;

    private GameObject player;
    private GameManager gm;

    private void Start() {
        gm = FindObjectOfType<GameManager>();

        Relocate();
	}

    private void Relocate() {
        player = GameObject.FindGameObjectWithTag("Player");

        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector2 pos = dir * Random.Range(minRadius, maxRadius);

        transform.position = pos + (Vector2)player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            PlayerController playerController = collision.GetComponent<PlayerController>();

            if (playerController == null) Debug.LogWarning("Player controller does not exist! Probably this has the player tag by accident!");
            else {
                //print("Player collected a power up!");
                gm.MoreEnemies();
                gm.IncreaseScore(score);

                playerController.Upgrade();
                playerController.Recharge(power);
                Relocate();
            }
        }
    }
}