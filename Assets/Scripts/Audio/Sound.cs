using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string clipName;

    [Range(0f, 1f)] public float volume;
    [Range(0f, 1f)] public float pitch;
    public AudioClip clip;

    public bool loop;

    [HideInInspector] public AudioSource source;
}
