using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MasterVolumeSliderFunctions : MonoBehaviour {

    public AudioMixer audioMixer;

    void Awake() {
        float val;
        audioMixer.GetFloat("MasterVolume", out val);
        GetComponent<Slider>().value = val;
    }

	public void setVolume(float volume) {
        audioMixer.SetFloat("MasterVolume", volume);
    }
}
