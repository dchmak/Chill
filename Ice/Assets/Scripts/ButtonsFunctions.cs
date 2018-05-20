/*
* Created by Daniel Mak
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsFunctions : MonoBehaviour {    

	public void LoadScene(string scene) {
        SceneManager.LoadSceneAsync(scene);
    }

    public void PlayAudio(string audio) {
        AudioController.instance.Play(audio);
    } 
}