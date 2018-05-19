/*
* Created by Daniel Mak
*/

using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class FreezeCircle : MonoBehaviour {

    [Range(0f, 100f)] public float damage;

    private void OnTriggerStay2D(Collider2D collision) {
        switch (collision.gameObject.tag) {
            case "Player":
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.TakeDamage(damage);
                break;

            case "Enemy":
                EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                enemy.TakeDamage(damage);
                break;

            default:
                break;
        }
    }
}