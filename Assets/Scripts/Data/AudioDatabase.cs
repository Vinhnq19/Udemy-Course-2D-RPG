using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Audio/Audio Database")]
public class AudioDatabase : ScriptableObject 
{
    public List<AudioClipData> player;
    public List<AudioClipData> uiAudio;

    [Header("Music Lists")]
    public List<AudioClipData> mainMenuMusic;
    public List<AudioClipData> levelMusic;

    private Dictionary<string, AudioClipData> clipCollection;

    private void OnEnable()
    {
        clipCollection = new Dictionary<string, AudioClipData>();
        AddToCollection(player);
        AddToCollection(uiAudio);
        AddToCollection(mainMenuMusic);
        AddToCollection(levelMusic);
    }

    public AudioClipData Get(string groupName)
    {
        return clipCollection.TryGetValue(groupName, out var audioData) ? audioData : null;
    }

    private void AddToCollection(List<AudioClipData> listToAdd)
    {
        foreach (var audioData in listToAdd)
        {
            if (audioData != null && !clipCollection.ContainsKey(audioData.audioName))
            {
                clipCollection.Add(audioData.audioName, audioData);
            }
        }
    }
}
[System.Serializable]
public class AudioClipData
{
    public string audioName;
    public List<AudioClip> clips = new List<AudioClip>();
    [Range(0f, 1f)] public float maxVolume = 1f;

    public AudioClip GetRandomClip()
    {
        if (clips.Count == 0 || clips == null)
            return null;

        int randomIndex = Random.Range(0, clips.Count);
        return clips[randomIndex];
    }
}