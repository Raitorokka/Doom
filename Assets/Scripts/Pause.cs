using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Pause : MonoBehaviour
{
    public AudioMixerSnapshot MainSnapshot;
    public AudioMixerSnapshot PauseSnapshot;
    public void OnEnable()
    {
        Time.timeScale = 0;
        PauseSnapshot.TransitionTo(0.001f);
    }
    public void OnDisable()
    {
        Time.timeScale = 1;
        MainSnapshot.TransitionTo(0.001f);
    }
}
