using UnityEngine;

public class AudioManager : MonoBehaviour, IDataPersistence
{
    public static AudioManager Instance;

    private float currentVolume;

    public Sound[] sounds;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance.gameObject);
        }
        DontDestroyOnLoad(Instance);
        
        InitializeSounds();
    }

    private void Start()
    {
        Play("Music");
    }

    private void InitializeSounds()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source = gameObject.AddComponent<AudioSource>();
            sounds[i].source.clip = sounds[i].clip;
            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.loop = sounds[i].loop;
        }
    }

    public void Play(string soundName)
    {
        Sound sound = System.Array.Find(sounds, s => s.clipName == soundName);
        if(sound != null)
        {
            sound.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound sound = System.Array.Find(sounds, s => s.clipName == name);
        if (sound != null)
        {
            sound.source.Stop();
        }
    }

    
    public void SetVolume(float value)
    {
        foreach (Sound sound in sounds)
        {
            sound.source.volume = value;
        }
        currentVolume = value;
    }

    public void LoadData(GameData data)
    {
        currentVolume = data.audioSliderValue;
        SetVolume(currentVolume);
    }

    public void SaveData(ref GameData data)
    {
        data.audioSliderValue = currentVolume;
    }
}
