/*
* Created by Daniel Mak
*/

using System.Collections.Generic;
using UnityEngine;

public class FreezeCone : MonoBehaviour {

    [Range(0f, 1f)] public float damage;
    //[Range(0f, 10f)] public float freezeNeededTime;

    private List<ParticleCollisionEvent> collisionEvents;
    private ParticleSystem ps;
    private GameObject player;

    private void Start () {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject collision) {
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

        ps = GetComponent<ParticleSystem>();

        ParticlePhysicsExtensions.GetCollisionEvents(ps, collision, collisionEvents);
        foreach (ParticleCollisionEvent collisionEvent in collisionEvents) {
            Component component = collisionEvent.colliderComponent;
            if (component == null) Debug.LogWarning("Component does not exist");
            else {
                GameObject gameObject = component.gameObject;
                if (gameObject == null) Debug.LogWarning("Game object does not exist");
                else {
                    EnemyController enemyController = gameObject.GetComponent<EnemyController>();

                    if (enemyController == null) Debug.LogWarning("Enemy controller does not exist!");
                    else enemyController.TakeDamage(damage * damageScaler);
                }
            }
            //EnemyController enemyController = collisionEvent.colliderComponent.gameObject.GetComponent<EnemyController>();

            //if (enemyController == null) Debug.LogWarning("Enemy controller does not exist!");
            //else enemyController.TakeDamage(damage * damageScaler);
        }
    }
}