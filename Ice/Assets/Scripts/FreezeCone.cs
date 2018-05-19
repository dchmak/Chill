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

    private void Start () {
        collisionEvents = new List<ParticleCollisionEvent>();
    }
	
	private void Update () {

    }

    private void OnParticleCollision(GameObject collision) {
        ps = GetComponent<ParticleSystem>();

        ParticlePhysicsExtensions.GetCollisionEvents(ps, collision, collisionEvents);
        foreach (ParticleCollisionEvent collisionEvent in collisionEvents) {
            EnemyController enemyController = collisionEvent.colliderComponent.gameObject.GetComponent<EnemyController>();

            enemyController.TakeDamage(damage);
        }
    }
}