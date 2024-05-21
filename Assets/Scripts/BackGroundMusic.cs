using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackGroundMusic : MonoBehaviour
{    
    public AudioMixerGroup Mixer;
    public AudioSettings AudioSettings;
    // Start is called before the first frame update
    void Start()
    {
        if (!Settings.BackGroundMusicPlaying)
        {
            DontDestroyOnLoad(gameObject);
            Settings.BackGroundMusicPlaying = true;
        }
        else
        {
            gameObject.SetActive(false);
        }     
     Mixer.audioMixer.SetFloat("MasterVolume", AudioSettings.MasterVolumeSound);
     Mixer.audioMixer.SetFloat("MasterSound", AudioSettings.MasterGameSound);
     Mixer.audioMixer.SetFloat("MasterMusic", AudioSettings.MasterMusicSound); 
    }    
    void Update()
    {
        
    }    
}
