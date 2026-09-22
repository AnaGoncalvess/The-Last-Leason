using UnityEngine;

/// <summary>
/// Singleton de áudio que se auto-instancia: nenhuma configuração na cena é necessária.
/// Outros scripts chamam AudioManager.Instance.PlaySfx(clip) e, se o clip for nulo, nada acontece.
/// </summary>
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("AudioManager (Auto)");
                instance = go.AddComponent<AudioManager>();
            }

            return instance;
        }
    }

    [Range(0f, 1f)] [SerializeField] private float sfxVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float musicVolume = 0.6f;

    private AudioSource sfxSource;
    private AudioSource musicSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
    }

    public void PlaySfx(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume * volumeScale);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public bool IsMuted { get; private set; }

    public void ToggleMute()
    {
        SetMuted(!IsMuted);
    }

    public void SetMuted(bool muted)
    {
        IsMuted = muted;
        // Mudo global (mais simples e confiável que zerar cada AudioSource individualmente).
        AudioListener.volume = muted ? 0f : 1f;
    }
}
