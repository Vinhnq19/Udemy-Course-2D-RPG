using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioDatabase audioDB;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private AudioClip lastMusicPlayerd;
    private string currentBgmGroupName;
    private Coroutine currentBgmCo;
    [SerializeField] private bool bgmShouldPlay;

    private Transform player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if(bgmSource.isPlaying == false && bgmShouldPlay)
        {
            if(string.IsNullOrEmpty(currentBgmGroupName) == false)
                
            NextBGM(currentBgmGroupName);
        }

        if(bgmSource.isPlaying && bgmShouldPlay == false)
        {
            StopBGM();
        }
    }

    public void StartBGM(string musicGroup)
    {
        bgmShouldPlay = true;

        if (musicGroup == currentBgmGroupName)
            return;
        
        NextBGM(musicGroup);

    }

    public void NextBGM(string musicGroup)
    {
        bgmShouldPlay = true;
        currentBgmGroupName = musicGroup;

        if (currentBgmCo != null)
            StopCoroutine(currentBgmCo);
        currentBgmCo = StartCoroutine(SwitchMusicCo(musicGroup));
    }

    public void StopBGM()
    {
        bgmShouldPlay = false;

        StartCoroutine(FadeVolumeCo(bgmSource, 0f, 1f));
        if(currentBgmCo != null)
        StopCoroutine(currentBgmCo);
    }

    private IEnumerator SwitchMusicCo(string musicGroup)
    {
        AudioClipData data = audioDB.Get(musicGroup);
        AudioClip nextMusic = data.GetRandomClip();

        if (data == null || data.GetRandomClip() == null)
            yield break;
        if (data.clips.Count > 1)
        {
            while (nextMusic == lastMusicPlayerd)
            {
                nextMusic = data.GetRandomClip();
            }
        }
        if (bgmSource.isPlaying)
            yield return FadeVolumeCo(bgmSource, 0f, 1f);

        lastMusicPlayerd = nextMusic;
        bgmSource.clip = nextMusic;
        bgmSource.volume = 0f;
        bgmSource.Play();
        StartCoroutine(FadeVolumeCo(bgmSource, data.maxVolume, 1f));
    }

    private IEnumerator FadeVolumeCo(AudioSource source, float targetVolume, float duration)
    {
        float time = 0f;
        float startVolume = source.volume;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }
        source.volume = targetVolume;
    }

    public void PlaySFX(string soundName, AudioSource sfxSource, float minDistanceToHearSound = 5)
    {
        if (player == null)
            player = Player.instance.transform;
        var data = audioDB.Get(soundName);
        if (data == null)
        {
            Debug.LogWarning($"Sound {soundName} not found in Audio Database.");
            return;
        }


        var clip = data.GetRandomClip();
        if (clip == null) return;

        float maxVolume = data.maxVolume;
        float distance = Vector2.Distance(sfxSource.transform.position, player.position);
        float t = Mathf.Clamp01(1 - (distance / minDistanceToHearSound));

        sfxSource.pitch = Random.Range(0.95f, 1.1f);
        sfxSource.volume = Mathf.Lerp(0, maxVolume, t * t);
        sfxSource.PlayOneShot(clip);


    }

    public void PlayGlobalSFX(string soundName)
    {
        var data = audioDB.Get(soundName);
        if (data == null)
        {

            return;
        }
        var clip = data.GetRandomClip();
        if (clip == null) return;

        sfxSource.pitch = Random.Range(0.95f, 1.1f);
        sfxSource.volume = data.maxVolume;
        sfxSource.PlayOneShot(clip);
    }
}
