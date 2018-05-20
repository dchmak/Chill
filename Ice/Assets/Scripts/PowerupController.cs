/*
* Created by Daniel Mak
*/

using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PowerupController : MonoBehaviour {

    public float minRadius, maxRadius;
    [Range(0f, 100f)] public float power;

    private void Start() {
        Relocate();
	}

    private void Relocate() {
        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector2 pos = dir * Random.Range(minRadius, maxRadius);

        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.isActiveAndEnabled) return;

        if (collision.gameObject.tag == "Player") {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player == null) Debug.LogWarning("Player controller does not exist! Probably this has the player tag by accident!");
            else {
                //print("Player collected a power up!");
                player.Upgrade();
                player.Recharge(power);
                Relocate();
            }
        }
    }
}