using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;
    public float LerpSpeed = .1f;

    public Sound[] sounds;

    [HideInInspector]
    public AudioSource lerpingAudioSource;
    bool StartLerping = false;
    float FinalVolume = 1;

	void Awake()
	{
        DontDestroyOnLoad(this.gameObject);
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.playOnAwake = s.awake;

		}
	}

	public void Play(string sound)
	{
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }

        s.source.Play();
	}

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }
    public void Pause(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Pause();
    }
    public void UnPause(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.UnPause();
    }

    public void SetVolume(string sound , float vol)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        lerpingAudioSource = s.source;
        FinalVolume = vol;
        StartLerping = true;
    }

    private void Update()
    {
        if(StartLerping == true && lerpingAudioSource != null)
        {
            lerpingAudioSource.volume = Mathf.Lerp(lerpingAudioSource.volume, FinalVolume, LerpSpeed);
            if(lerpingAudioSource.volume == FinalVolume)
            {
                StartLerping = false;
            }
        }
    }

}
