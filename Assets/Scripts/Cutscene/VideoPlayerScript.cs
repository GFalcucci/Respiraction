using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Localization.Settings;

public class VideoPlayerScript : MonoBehaviour
{
    [Tooltip("Componente de VideoPlayer")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Vídeo de cada idioma")]
    [Tooltip("Vídeo em português (também é usado se faltar o de outro idioma)")]
    [SerializeField] private VideoClip videoPt;
    [Tooltip("Vídeo em inglês")]
    [SerializeField] private VideoClip videoEn;
    [Tooltip("Vídeo em espanhol")]
    [SerializeField] private VideoClip videoEs;

    [Tooltip("Define a cena a ser carregada após o vídeo")]
    [SerializeField] private string nextSceneName = "Menu";

    [Tooltip("Objeto do botao de pausa")]
    [SerializeField] private GameObject botaoPausa;

    [Tooltip("Objeto do botao de resumir")]
    [SerializeField] private GameObject botaoResumir;

    IEnumerator Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("[CUTSCENE] VideoPlayer não está configurado. Atribua o componente VideoPlayer no Inspector.");
            yield break;
        }

        // Espera o sistema de Localization terminar de iniciar para saber o idioma
        yield return LocalizationSettings.InitializationOperation;

        VideoClip escolhido = EscolherVideo();
        if (escolhido == null)
        {
            Debug.LogError("[CUTSCENE] Nenhum vídeo foi atribuído. Preencha pelo menos o vídeo em português.");
            yield break;
        }

        // Assina o evento para quando o vídeo terminar
        videoPlayer.loopPointReached += OnVideoEnd;

        // Troca o vídeo pelo do idioma atual e começa do início
        videoPlayer.Stop();
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = escolhido;
        videoPlayer.time = 0;
        videoPlayer.Play();
    }

    // Escolhe o vídeo conforme o idioma selecionado no Localization
    private VideoClip EscolherVideo()
    {
        var locale = LocalizationSettings.SelectedLocale;
        if (locale == null) return videoPt;

        string code = locale.Identifier.Code; // ex: "pt", "en-US", "es-ES"

        if (code.StartsWith("en") && videoEn != null) return videoEn;
        if (code.StartsWith("es") && videoEs != null) return videoEs;
        return videoPt;
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("[CUTSCENE] Vídeo de introdução terminou. Carregando a cena: " + nextSceneName);

        // Carrega a próxima cena
        SceneManager.LoadScene(nextSceneName);
    }

    public void OnPause()
    {
        Debug.Log("[CUTSCENE] Pausando vídeo");
        botaoPausa.SetActive(false);
        botaoResumir.SetActive(true);
        videoPlayer.Pause();
    }

    public void OnResume()
    {
        Debug.Log("[CUTSCENE] Resumindo vídeo");
        botaoPausa.SetActive(true);
        botaoResumir.SetActive(false);
        videoPlayer.Play();
    }

    public void OnRestart()
    {
        Debug.Log("[CUTSCENE] Reiniciando vídeo");
        videoPlayer.time = 0;
        botaoPausa.SetActive(true);
        botaoResumir.SetActive(false);
        videoPlayer.Play();
    }
}