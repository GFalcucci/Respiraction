using UnityEngine;
using UnityEngine.Localization.Settings;

// Este script é apenas um "container" de dados.
// Ele diz "Eu sou interativo, este é o meu nome e a minha descrição" em cada idioma.
public class InteractablePart : MonoBehaviour
{
    [Header("Nome do Tooltip")]
    [Tooltip("O nome que aparecerá no tooltip (Português)")]
    public string tooltipName = "Nome da Parte";
    [Tooltip("Tooltip name (English)")]
    public string tooltipNameEng;
    [Tooltip("Nombre del tooltip (Español)")]
    public string tooltipNameEsp;

    [Header("Descrição Completa")]
    [Tooltip("O texto que aparecerá quando o objeto for clicado (Português)")]
    [TextArea(5, 10)]
    public string description = "Insira a descrição aqui...";
    [Tooltip("Description shown when the object is clicked (English)")]
    [TextArea(5, 10)]
    public string descriptionEng;
    [Tooltip("Descripción que aparece al hacer clic (Español)")]
    [TextArea(5, 10)]
    public string descriptionEsp;

    [Header("Áudio")]
    [Tooltip("Número de ID do audio a ser tocado no audioManagerScene")]
    public int audioNum;

    // Nome no idioma atual (usa o português se a tradução estiver vazia)
    public string GetTooltipName()
    {
        return Escolher(tooltipName, tooltipNameEng, tooltipNameEsp);
    }

    // Descrição no idioma atual (usa o português se a tradução estiver vazia)
    public string GetDescription()
    {
        return Escolher(description, descriptionEng, descriptionEsp);
    }

    private static string Escolher(string pt, string en, string es)
    {
        var locale = LocalizationSettings.SelectedLocale;
        if (locale == null) return pt;

        string code = locale.Identifier.Code; // ex: "pt", "en-US", "es-ES"

        if (code.StartsWith("en") && !string.IsNullOrEmpty(en)) return en;
        if (code.StartsWith("es") && !string.IsNullOrEmpty(es)) return es;
        return pt;
    }
}