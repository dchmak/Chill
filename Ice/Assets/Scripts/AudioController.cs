using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour {

    #region Sound Class
    [System.Serializable]
    public class Sound {
        public string name;

        public AudioClip clip;
        public AudioMixerGroup mixer;

        [Range(0f, 1f)]
        public float volume;

        [Range(0.1f, 3f)]
        public float pitch;

        [HideInInspector]
        public AudioSource source;

        public bool loop;
    }
    #endregion

    public Sound[] sounds;

    public static AudioController instance;

	void Awake () {
        if (instance == null) instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.outputAudioMixerGroup = sound.mixer;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
	}

    private void Start() {
        Play("Background");
    }

    public void Play (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound " + name + " does not exist.");
            return;
        }

        s.source.Play();
    }

    public void Stop () {
        foreach (Sound sound in sounds) {
            sound.source.Stop();
        }
    }

    public void Stop(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound " + name + " does not exist.");
        }
        s.source.Stop();
    }

    public bool IsPlaying(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound " + name + " does not exist.");
            return false;
        }
        return s.source.isPlaying;
    } 

    public AudioSource CurrentlyPlaying() {
        foreach (Sound sound in sounds) {
            if (sound.source.isPlaying) return sound.source;
        }
        return null;
    }
}
