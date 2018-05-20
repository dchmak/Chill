/*
* Created by Daniel Mak
*/

using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class FreezeCircle : MonoBehaviour {

    [Range(0f, 1f)] public float damage;

    private GameObject player;

    private void OnTriggerStay2D(Collider2D collision) {
        float damageScaler = 1f;

        player = GameObject.FindGameObjectWithTag("Player");
        PlayerController controller = player.GetComponent<PlayerController>();

        if (controller == null) Debug.LogWarning("Player controller does not exist!");
        else {
            PlayerController.Upgradable damageUpgrade = controller.FindUpgradable("Damage");
            if (damageUpgrade == null) Debug.LogWarning("The upgrade does not exist!");
            else {
                damageScaler = damageUpgrade.GetScaler();
            }
        }

        switch (collision.gameObject.tag) {
            case "Player":
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.TakeDamage(damage * damageScaler);
                break;

            case "Enemy":
                EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                enemy.TakeDamage(damage * damageScaler);
                break;

            default:
                break;
        }
    }
}