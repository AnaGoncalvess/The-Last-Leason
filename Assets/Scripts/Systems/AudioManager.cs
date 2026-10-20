// using UnityEngine;

// /// <summary>
// /// Singleton de áudio que se auto-instancia: nenhuma configuração na cena é necessária.
// /// Outros scripts chamam AudioManager.Instance.PlaySfx(clip) e, se o clip for nulo, nada acontece.
// /// </summary>
// public class AudioManager : MonoBehaviour
// {
//     private static AudioManager instance;

//     public static AudioManager Instance
//     {
//         get
//         {
//             if (instance == null)
//             {
//                 GameObject go = new GameObject("AudioManager (Auto)");
//                 instance = go.AddComponent<AudioManager>();
//             }

//             return instance;
//         }
//     }

//     [Range(0f, 1f)] [SerializeField] private float sfxVolume = 1f;
//     [Range(0f, 1f)] [SerializeField] private float musicVolume = 0.6f;

//     private AudioSource sfxSource;
//     private AudioSource musicSource;

//     private void Awake()
//     {
//         if (instance != null && instance != this)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         instance = this;
//         DontDestroyOnLoad(gameObject);

//         sfxSource = gameObject.AddComponent<AudioSource>();
//         sfxSource.playOnAwake = false;

//         musicSource = gameObject.AddComponent<AudioSource>();
//         musicSource.playOnAwake = false;
//         musicSource.loop = true;
//     }

//     public void PlaySfx(AudioClip clip, float volumeScale = 1f)
//     {
//         if (clip == null) return;
//         sfxSource.PlayOneShot(clip, sfxVolume * volumeScale);
//     }

//     public void PlayMusic(AudioClip clip, bool loop = true)
//     {
//         if (clip == null) return;
//         if (musicSource.clip == clip && musicSource.isPlaying) return;

//         musicSource.clip = clip;
//         musicSource.loop = loop;
//         musicSource.volume = musicVolume;
//         musicSource.Play();
//     }

//     public void StopMusic()
//     {
//         musicSource.Stop();
//     }

//     public bool IsMuted { get; private set; }

//     public void ToggleMute()
//     {
//         SetMuted(!IsMuted);
//     }

//     public void SetMuted(bool muted)
//     {
//         IsMuted = muted;
//         // Mudo global (mais simples e confiável que zerar cada AudioSource individualmente).
//         AudioListener.volume = muted ? 0f : 1f;
//     }
// }

// using UnityEngine;
// using UnityEngine.UI;

// public class AudioManager : MonoBehaviour
// {
//     public static AudioManager Instance { get; private set; }

//     [Header("Configurações do Botão de Áudio")]
//     [SerializeField] private Button audioButton;
//     [SerializeField] private Image buttonImage;
//     [SerializeField] private Sprite soundOnSprite;
//     [SerializeField] private Sprite soundOffSprite;

//     private bool isMuted = false;

//     private void Awake()
//     {
//         // Garante que o AudioManager persista e seja único entre as cenas
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//             return;
//         }
//     }

//     private void Start()
//     {
//         // Carrega o estado do som guardado anteriormente (0 = Som Ligado, 1 = Mutado)
//         isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
//         AplicarEstadoAudio();

//         if (audioButton != null)
//         {
//             audioButton.onClick.AddListener(ToggleAudio);
//         }
//     }

//     public void ToggleAudio()
//     {
//         isMuted = !isMuted;

//         // Guarda a preferência do jogador
//         PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
//         PlayerPrefs.Save();

//         AplicarEstadoAudio();
//     }

//     private void AplicarEstadoAudio()
//     {
//         // Desliga/Liga o áudio global do jogo (AudioListener)
//         AudioListener.pause = isMuted;
//         AudioListener.volume = isMuted ? 0f : 1f;

//         // Atualiza o sprite do botão
//         if (buttonImage != null)
//         {
//             if (isMuted && soundOffSprite != null)
//             {
//                 buttonImage.sprite = soundOffSprite;
//             }
//             else if (!isMuted && soundOnSprite != null)
//             {
//                 buttonImage.sprite = soundOnSprite;
//             }
//         }
//     }

//     // Método para re-vincular o botão caso o Canvas seja recarregado
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
// }

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

//     private AudioSource sfxSource;
//     private AudioSource musicSource;

//     private Coroutine duckingCoroutine;
//     public bool IsMuted { get; private set; }

//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);

//             // Cria automaticamente os AudioSources para SFX e Música
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

// [SerializeField] private AudioClip musicaFase;


//     // private void Start()
//     // {
// //         // Carrega o estado de mudo guardado no PlayerPrefs (0 = Ligado, 1 = Mutado)
// //         IsMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
// //         AplicarEstadoAudio();

// //         if (audioButton != null)
// //         {
// //             audioButton.onClick.AddListener(ToggleAudio);
// //         }

// //         if (AudioManager.Instance != null && musicaDeFundo != null)
// //         {
// //             AudioManager.Instance.PlayMusic(musicaDeFundo);
// //         }

// //   if (musicaFase != null)
// //     {
// //         PlayMusic(musicaFase);
// //     }

// private void Start()
// {
//     IsMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
    
//     // Toca a música atribuída no campo Musica De Fundo ao iniciar
//     if (musicaDeFundo != null)
//     {
//         PlayMusic(musicaDeFundo);
//     }

//     AplicarEstadoAudio();

//     if (audioButton != null)
//     {
//         audioButton.onClick.AddListener(ToggleAudio);
//     }

      
//     }

//     // --- MÉTODOS DE ÁUDIO (Chamados pelo Player, Caderno, Coletáveis) ---
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
//         Debug.Log("O BOTÃO DE SOM FOI CLICADO!"); // <--- MENSAGEM DE TESTE
//         ToggleMute();
//     }

//     public void ToggleMute()
//     {
//         IsMuted = !IsMuted;
//         PlayerPrefs.SetInt("IsMuted", IsMuted ? 1 : 0);
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
//     [SerializeField] private AudioClip musicaDeFundo;

//     // private void AplicarEstadoAudio()
//     // {
//     //     // 1. Desliga o AudioListener global
//     //     AudioListener.pause = IsMuted;
//     //     AudioListener.volume = IsMuted ? 0f : 1f;

//     //     // 2. Muta todos os AudioSources anexados a este GameObject
//     //     AudioSource[] todosAudioSources = GetComponents<AudioSource>();
//     //     foreach (AudioSource source in todosAudioSources)
//     //     {
//     //         source.mute = IsMuted;
//     //     }

//     //     // 3. Atualiza o ícone visual do botão
//     //     if (buttonImage != null)
//     //     {
//     //         Sprite spriteAlvo = IsMuted ? soundOffSprite : soundOnSprite;
//     //         if (spriteAlvo != null)
//     //         {
//     //             buttonImage.sprite = spriteAlvo;
//     //             buttonImage.SetAllDirty();
//     //         }
//     //     }

// private void AplicarEstadoAudio()
// {
//     // 1. Pausa o ouvinte global da Unity
//     AudioListener.pause = IsMuted;
//     AudioListener.volume = IsMuted ? 0f : 1f;

//     // 2. Muta diretamente as fontes de áudio do AudioManager
//     if (sfxSource != null) sfxSource.mute = IsMuted;
//     if (musicSource != null) musicSource.mute = IsMuted;

//     // 3. Troca o Sprite do botão na UI
//     if (buttonImage != null)
//     {
//         Sprite spriteAlvo = IsMuted ? soundOffSprite : soundOnSprite;
//         if (spriteAlvo != null)
//         {
//             buttonImage.sprite = spriteAlvo;
//             buttonImage.SetAllDirty();
//         }
//     }
// }
    

//     // --- EFEITO DUCK AUDIO (Abaixar a música ao coletar item) ---
//     public void PlaySfxWithDucking(AudioClip clip, float duckedVolume = 0.2f, float duckDuration = 0.5f)
//     {
//         if (clip == null) return;

//         PlaySfx(clip);

//         if (duckingCoroutine != null)
//         {
//             StopCoroutine(duckingCoroutine);
//         }

//         duckingCoroutine = StartCoroutine(RutinaAbaixarMusica(duckedVolume, duckDuration));
//     }

//     private IEnumerator RutinaAbaixarMusica(float duckedVolume, float duration)
//     {
//         if (musicSource == null) yield break;

//         float volumeOriginal = musicVolume;
//         musicSource.volume = volumeOriginal * duckedVolume;

//         yield return new WaitForSeconds(duration);

//         float tempoSurgimento = 0.3f;
//         float tempoDecorrido = 0f;
//         float volumeAtual = musicSource.volume;

//         while (tempoDecorrido < tempoSurgimento)
//         {
//             tempoDecorrido += Time.deltaTime;
//             musicSource.volume = Mathf.Lerp(volumeAtual, volumeOriginal, tempoDecorrido / tempoSurgimento);
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
        // Permite testar/clicar na caixa Is Muted diretamente pelo Inspector
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

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null) musicSource.Stop();
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
        float tempoFade = 0.15f; // Tempo da transição (suave)
        float tempoDecorrido = 0f;

        // 1. Transição Suave (Fade Out) para abaixar o som
        while (tempoDecorrido < tempoFade)
        {
            tempoDecorrido += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(volumeOriginal, volumeAlvo, tempoDecorrido / tempoFade);
            yield return null;
        }
        musicSource.volume = volumeAlvo;

        // 2. Mantém o som mais baixo pela duração do efeito
        yield return new WaitForSeconds(duration);

        // 3. Transição Suave (Fade In) para restaurar o volume da música
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