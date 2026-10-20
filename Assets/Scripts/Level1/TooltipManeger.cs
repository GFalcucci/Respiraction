using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    // Referência para os elementos de UI criados no Editor
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;

    private Camera mainCamera;

    // Traduções de cada peça: [0] = Português, [1] = English, [2] = Español
    private static readonly Dictionary<string, string[]> descricoes = new Dictionary<string, string[]>
    {
        { "Tendao",          new[] { "Tendão Central",         "Central Tendon",          "Tendón Central" } },
        { "Trapezio",        new[] { "Trapézio",               "Trapezius",               "Trapecio" } },
        { "CostalDir",       new[] { "Costal Esquerdo",        "Left Costal",             "Costal Izquierdo" } },
        { "CostalEsq",       new[] { "Costal Direito",         "Right Costal",            "Costal Derecho" } },
        { "Esternal",        new[] { "Esternal",               "Sternal",                 "Esternal" } },
        { "EsternoCleidoM1", new[] { "Esternocleidomastóideo", "Sternocleidomastoid",     "Esternocleidomastoideo" } },
        { "EsternoCleidoM2", new[] { "Esternocleidomastóideo", "Sternocleidomastoid",     "Esternocleidomastoideo" } },
        { "Intercostais",    new[] { "Intercostais Externos",  "External Intercostals",   "Intercostales Externos" } },
        { "Lombar",          new[] { "Lombar",                 "Lumbar",                  "Lumbar" } },
        { "OblExterno",      new[] { "Oblíquo Externo",        "External Oblique",        "Oblicuo Externo" } },
        { "PsoasMaior",      new[] { "Psoas Maior",            "Psoas Major",             "Psoas Mayor" } },
        { "QuadLombar",      new[] { "Quadrado Lombar",        "Quadratus Lumborum",      "Cuadrado Lumbar" } },
    };

    void Start()
    {
        mainCamera = Camera.main;

        // Garante que o painel comece desativado
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Cria um raio a partir da posição do mouse na tela
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Só mostra o tooltip se o objeto tiver a tag "Peca"
            if (hit.collider.CompareTag("Peca"))
            {
                tooltipPanel.SetActive(true);

                // O painel segue o mouse, com um deslocamento para o cursor não ficar sobre ele
                tooltipPanel.transform.position = Input.mousePosition + new Vector3(-200, -75, 0);

                SetTooltipText(hit.collider.gameObject.name);
            }
            else
            {
                tooltipPanel.SetActive(false);
            }
        }
        else
        {
            tooltipPanel.SetActive(false);
        }
    }

    // Devolve 0 (português), 1 (inglês) ou 2 (espanhol) conforme o idioma selecionado
    private int GetLanguageIndex()
    {
        var locale = LocalizationSettings.SelectedLocale;
        if (locale == null) return 0;

        string code = locale.Identifier.Code; // ex: "pt", "en-US", "es-ES"
        if (code.StartsWith("en")) return 1;
        if (code.StartsWith("es")) return 2;
        return 0;
    }

    // Define o texto do tooltip com base no nome da peça e no idioma atual
    private void SetTooltipText(string pieceName)
    {
        if (descricoes.TryGetValue(pieceName, out string[] traducoes))
        {
            tooltipText.text = traducoes[GetLanguageIndex()];
        }
        else
        {
            // Peça não encontrada na lista
            tooltipText.text = "";
        }
    }
}