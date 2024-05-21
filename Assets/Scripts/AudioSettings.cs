using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Settings", menuName = "Create Audio Settings")]
public class AudioSettings : ScriptableObject
{
    public float MasterVolumeSound;
    public float MasterMusicSound;
    public float MasterGameSound;
}

