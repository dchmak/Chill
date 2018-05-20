/*
* Created by Daniel Mak
*/

using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject powerupPrefab;
    [Range(1f, 100f)] public float numberOfPowerup;

    public GameObject enemyPrefab;
    [Range(1f, 100f)] public float numberOfEnemy;

    public GameObject player;

    private GameObject powerupsHolder;
    private List<GameObject> powerups;
    private GameObject enemiesHolder;
    private List<GameObject> enemies;

    private void Start () {
        powerupsHolder = new GameObject {
            name = "Powerups Holder"
        };
        powerups = new List<GameObject>();

        enemiesHolder = new GameObject {
            name = "Enemies Holder"
        };
        enemies = new List<GameObject>();

        player.SetActive(false);

        for (int i = 0; i < numberOfPowerup; i++) {
            GameObject powerup = Instantiate(powerupPrefab, new Vector2(10, 10), Quaternion.identity);
            powerup.transform.SetParent(powerupsHolder.transform);
            powerups.Add(powerup);
        }

        player.SetActive(true);
    }
}