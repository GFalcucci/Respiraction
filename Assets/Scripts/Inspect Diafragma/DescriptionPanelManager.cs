using UnityEngine;
using TMPro; // Para TextMeshPro
using UnityEngine.UI; // Para o Botão
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class DescriptionPanelManager : MonoBehaviour
{
    // Singleton: Uma forma de acessar este script de qualquer lugar
    public static DescriptionPanelManager Instance { get; private set; }

    [Header("UI do Painel")]
    public GameObject descriptionPanelObject;
    public TextMeshProUGUI descriptionText;
    public Button closeButton;

    // Peça cuja descrição está aberta (para traduzir e atualizar ao trocar de idioma)
    private InteractablePart parteAtual;

    void Awake()
    {
        // Configuração do Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += AoTrocarIdioma;
    }

    void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= AoTrocarIdioma;
    }

    void Start()
    {
        // Adiciona a função 'HidePanel' ao clique do botão
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HidePanel);
        }
        // Garante que o painel começa desligado
        descriptionPanelObject.SetActive(false);
    }

    // Mostra o painel a partir da peça (forma mais direta)
    public void ShowPanel(InteractablePart parte)
    {
        parteAtual = parte;
        descriptionText.text = parte.GetDescription();
        descriptionPanelObject.SetActive(true);
    }

    // Mostra o painel a partir do texto em português (é o que o script do clique já envia).
    // Procura a peça que tem essa descrição e usa a tradução dela.
    public void ShowPanel(string newDescription)
    {
        parteAtual = EncontrarParte(newDescription);

        descriptionText.text = (parteAtual != null)
            ? parteAtual.GetDescription()
            : newDescription; // se não achar a peça, mostra o texto como veio

        descriptionPanelObject.SetActive(true);
    }

    // Função para esconder o painel (usada pelo botão)
    public void HidePanel()
    {
        parteAtual = null;
        descriptionPanelObject.SetActive(false);
    }

    // Função para verificar se o painel está aberto
    public bool IsPanelOpen()
    {
        return descriptionPanelObject.activeSelf;
    }

    // Se trocar o idioma com o painel aberto, atualiza o texto na hora
    private void AoTrocarIdioma(Locale novoIdioma)
    {
        if (parteAtual != null && descriptionPanelObject.activeSelf)
        {
            descriptionText.text = parteAtual.GetDescription();
        }
    }

    // Acha a peça da cena cuja descrição em português é igual ao texto recebido
    private InteractablePart EncontrarParte(string descricaoPt)
    {
        var partes = FindObjectsByType<InteractablePart>(FindObjectsSortMode.None);
        foreach (var p in partes)
        {
            if (p.description == descricaoPt) return p;
        }
        return null;
    }
}