/*
* Created by Daniel Mak
*/

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameoverScoresController : MonoBehaviour {

    public TextMeshProUGUI score;

    private void Awake() {
        score.text = "Score: " + GameManager.score.ToString("F0");
    }
}
