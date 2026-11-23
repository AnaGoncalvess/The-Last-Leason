// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;

// public class AudioManager : MonoBehaviour
// {
//     public static AudioManager Instance { get; private set; }

//     [Header("Configurações do Botão de Áudio (UI)")]
//     [SerializeField] private Button audioButton;
//     [SerializeField] private Image buttonImage;
//     [SerializeField] private Sprite soundOnSprite;
//     [SerializeField] private Sprite soundOffSprite;

//     [Header("Volumes Gerais")]
//     [Range(0f, 1f)][SerializeField] private float sfxVolume = 1f;
//     [Range(0f, 1f)][SerializeField] private float musicVolume = 0.6f;

//     [Header("Músicas da Fase")]
//     [SerializeField] private AudioClip musicaDeFundo;

//     [Header("Estado do Som")]
//     [SerializeField] private bool isMuted = false;

//     private AudioSource sfxSource;
//     private AudioSource musicSource;
//     private Coroutine duckingCoroutine;

//     public bool IsMuted => isMuted;

//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);

//             sfxSource = gameObject.AddComponent<AudioSource>();
//             sfxSource.playOnAwake = false;

//             musicSource = gameObject.AddComponent<AudioSource>();
//             musicSource.playOnAwake = false;
//             musicSource.loop = true;
//         }
//         else
//         {
//             Destroy(gameObject);
//             return;
//         }
//     }

//     private void Start()
//     {
//         isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;

//         if (musicaDeFundo != null)
//         {
//             PlayMusic(musicaDeFundo);
//         }

//         AplicarEstadoAudio();

//         if (audioButton != null)
//         {
//             audioButton.onClick.RemoveAllListeners();
//             audioButton.onClick.AddListener(ToggleAudio);
//         }
//     }

//     private void OnValidate()
//     {
//         // Permite testar/clicar na caixa Is Muted diretamente pelo Inspector
//         if (Application.isPlaying)
//         {
//             AplicarEstadoAudio();
//         }
//     }

//     // --- MÉTODOS DE ÁUDIO ---
//     public void PlaySfx(AudioClip clip, float volumeScale = 1f)
//     {
//         if (clip == null || sfxSource == null) return;
//         sfxSource.PlayOneShot(clip, sfxVolume * volumeScale);
//     }

//     public void PlayMusic(AudioClip clip, bool loop = true)
//     {
//         if (clip == null || musicSource == null) return;
//         if (musicSource.clip == clip && musicSource.isPlaying) return;

//         musicSource.clip = clip;
//         musicSource.loop = loop;
//         musicSource.volume = musicVolume;
//         musicSource.Play();
//     }

//     public void StopMusic()
//     {
//         if (musicSource != null) musicSource.Stop();
//     }

//     // --- MÉTODOS DO BOTÃO MUTE / UI ---
//     public void ToggleAudio()
//     {
//         ToggleMute();
//     }

//     public void ToggleMute()
//     {
//         isMuted = !isMuted;
//         PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
//         PlayerPrefs.Save();

//         AplicarEstadoAudio();
//     }

//     public void ConfigurarBotaoUI(Button novoBotao, Image novaImagem)
//     {
//         audioButton = novoBotao;
//         buttonImage = novaImagem;

//         if (audioButton != null)
//         {
//             audioButton.onClick.RemoveAllListeners();
//             audioButton.onClick.AddListener(ToggleAudio);
//         }

//         AplicarEstadoAudio();
//     }

//     private void AplicarEstadoAudio()
//     {
//         AudioListener.pause = isMuted;
//         AudioListener.volume = isMuted ? 0f : 1f;

//         if (sfxSource != null) sfxSource.mute = isMuted;
//         if (musicSource != null) musicSource.mute = isMuted;

//         if (buttonImage != null)
//         {
//             Sprite spriteAlvo = isMuted ? soundOffSprite : soundOnSprite;
//             if (spriteAlvo != null)
//             {
//                 buttonImage.sprite = spriteAlvo;
//                 buttonImage.SetAllDirty();
//             }
//         }
//     }

//     // --- TRANSITION SUAVE DO DUCK AUDIO ---
//     public void PlaySfxWithDucking(AudioClip clip, float duckedVolume = 0.2f, float duckDuration = 0.5f)
//     {
//         if (clip == null) return;

//         PlaySfx(clip);

//         if (duckingCoroutine != null)
//         {
//             StopCoroutine(duckingCoroutine);
//         }

//         duckingCoroutine = StartCoroutine(RutinaAbaixarMusicaSuave(duckedVolume, duckDuration));
//     }

//     private IEnumerator RutinaAbaixarMusicaSuave(float duckedVolume, float duration)
//     {
//         if (musicSource == null) yield break;

//         float volumeOriginal = musicVolume;
//         float volumeAlvo = volumeOriginal * duckedVolume;
//         float tempoFade = 0.15f; // Tempo da transição (suave)
//         float tempoDecorrido = 0f;

//         // 1. Transição Suave (Fade Out) para abaixar o som
//         while (tempoDecorrido < tempoFade)
//         {
//             tempoDecorrido += Time.deltaTime;
//             musicSource.volume = Mathf.Lerp(volumeOriginal, volumeAlvo, tempoDecorrido / tempoFade);
//             yield return null;
//         }
//         musicSource.volume = volumeAlvo;

//         // 2. Mantém o som mais baixo pela duração do efeito
//         yield return new WaitForSeconds(duration);

//         // 3. Transição Suave (Fade In) para restaurar o volume da música
//         tempoDecorrido = 0f;
//         while (tempoDecorrido < tempoFade)
//         {
//             tempoDecorrido += Time.deltaTime;
//             musicSource.volume = Mathf.Lerp(volumeAlvo, volumeOriginal, tempoDecorrido / tempoFade);
//             yield return null;
//         }

//         musicSource.volume = volumeOriginal;
//         duckingCoroutine = null;
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Configurações do Botão de Áudio (UI)")]
    [SerializeField] private Button audioButton;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    [Header("Volumes Gerais")]
    [Range(0f, 1f)][SerializeField] private float sfxVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float musicVolume = 0.6f;

    [Header("Músicas da Fase")]
    [SerializeField] private AudioClip musicaDeFundo;

    [Header("Estado do Som")]
    [SerializeField] private bool isMuted = false;

    private AudioSource sfxSource;
    private AudioSource musicSource;
    private Coroutine duckingCoroutine;

    public bool IsMuted => isMuted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;

            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true;
        }
        else
        {
            // Se a nova cena tiver um AudioManager com outra música configurada, troca para a música da nova fase!
            if (musicaDeFundo != null)
            {
                Instance.PlayMusic(musicaDeFundo);
            }
            else
            {
                Instance.StopMusic();
            }

            // Atualiza também a referência do botão de som da nova cena
            if (audioButton != null && buttonImage != null)
            {
                Instance.ConfigurarBotaoUI(audioButton, buttonImage);
            }

            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;

        if (musicaDeFundo != null)
        {
            PlayMusic(musicaDeFundo);
        }

        AplicarEstadoAudio();

        if (audioButton != null)
        {
            audioButton.onClick.RemoveAllListeners();
            audioButton.onClick.AddListener(ToggleAudio);
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            AplicarEstadoAudio();
        }
    }

    // --- MÉTODOS DE ÁUDIO ---
    public void PlaySfx(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume * volumeScale);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null || musicSource == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            musicSource.clip = null;
        }
    }

    // --- MÉTODOS DO BOTÃO MUTE / UI ---
    public void ToggleAudio()
    {
        ToggleMute();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        AplicarEstadoAudio();
    }

    public void ConfigurarBotaoUI(Button novoBotao, Image novaImagem)
    {
        audioButton = novoBotao;
        buttonImage = novaImagem;

        if (audioButton != null)
        {
            audioButton.onClick.RemoveAllListeners();
            audioButton.onClick.AddListener(ToggleAudio);
        }

        AplicarEstadoAudio();
    }

    private void AplicarEstadoAudio()
    {
        AudioListener.pause = isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;

        if (sfxSource != null) sfxSource.mute = isMuted;
        if (musicSource != null) musicSource.mute = isMuted;

        if (buttonImage != null)
        {
            Sprite spriteAlvo = isMuted ? soundOffSprite : soundOnSprite;
            if (spriteAlvo != null)
            {
                buttonImage.sprite = spriteAlvo;
                buttonImage.SetAllDirty();
            }
        }
    }

    // --- TRANSITION SUAVE DO DUCK AUDIO ---
    public void PlaySfxWithDucking(AudioClip clip, float duckedVolume = 0.2f, float duckDuration = 0.5f)
    {
        if (clip == null) return;

        PlaySfx(clip);

        if (duckingCoroutine != null)
        {
            StopCoroutine(duckingCoroutine);
        }

        duckingCoroutine = StartCoroutine(RutinaAbaixarMusicaSuave(duckedVolume, duckDuration));
    }

    private IEnumerator RutinaAbaixarMusicaSuave(float duckedVolume, float duration)
    {
        if (musicSource == null) yield break;

        float volumeOriginal = musicVolume;
        float volumeAlvo = volumeOriginal * duckedVolume;
        float tempoFade = 0.15f;
        float tempoDecorrido = 0f;

        while (tempoDecorrido < tempoFade)
        {
            tempoDecorrido += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(volumeOriginal, volumeAlvo, tempoDecorrido / tempoFade);
            yield return null;
        }
        musicSource.volume = volumeAlvo;

        yield return new WaitForSeconds(duration);

        tempoDecorrido = 0f;
        while (tempoDecorrido < tempoFade)
        {
            tempoDecorrido += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(volumeAlvo, volumeOriginal, tempoDecorrido / tempoFade);
            yield return null;
        }

        musicSource.volume = volumeOriginal;
        duckingCoroutine = null;
    }
}