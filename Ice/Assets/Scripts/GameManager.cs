/*
* Created by Daniel Mak
*/

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public static float score;

    public GameObject powerupPrefab;
    [Range(1f, 100f)] public float numberOfPowerups;

    public GameObject enemyPrefab;
    [Range(1f, 100f)] public float initialNumberOfEnemies;

    public GameObject player;

    public TextMeshProUGUI scoreText;

    private float startTime;
    private float bonusScore;
    private float numberOfEnemies;
    private GameObject powerupsHolder;
    private List<GameObject> powerups;
    private GameObject enemiesHolder;
    private List<GameObject> enemies;

    public float GetTimeElapsed() {
        return Time.time - startTime;
    }

    public void MoreEnemies() {
        numberOfEnemies++;
    }

    public void IncreaseScore(float increase) {
        bonusScore += increase;
    }

    private void Start () {
        startTime = Time.time;
        bonusScore = 0;

        powerupsHolder = new GameObject {
            name = "Powerups Holder"
        };
        powerups = new List<GameObject>();

        enemiesHolder = new GameObject {
            name = "Enemies Holder"
        };
        enemies = new List<GameObject>();

        player.SetActive(false);

        for (int i = 0; i < numberOfPowerups; i++) {
            GameObject powerup = Instantiate(powerupPrefab, new Vector2(10, 10), Quaternion.identity);
            powerup.transform.SetParent(powerupsHolder.transform);
            powerups.Add(powerup);
        }

        for (int i = 0; i < initialNumberOfEnemies; i++) {
            GameObject enemy = Instantiate(enemyPrefab, new Vector2(10, 10), Quaternion.identity);
            enemy.transform.SetParent(enemiesHolder.transform);
            enemies.Add(enemy);
        }

        player.SetActive(true);
    }

    private void Update() {
        float enemyExist = enemies.Count;
        if (numberOfEnemies > enemyExist) {
            GameObject enemy = Instantiate(enemyPrefab, new Vector2(10, 10), Quaternion.identity);
            enemy.transform.SetParent(powerupsHolder.transform);
            enemies.Add(enemy);
        }
    }

    private void LateUpdate() {
        score = GetTimeElapsed() + bonusScore;

        scoreText.text = "Score: " + score.ToString("F0");
    }
}