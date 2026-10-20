using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SeletorIdioma : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropdown;

    void Start()
    {
        StartCoroutine(Preparar());
    }

    System.Collections.IEnumerator Preparar()
    {
        // espera o sistema de localization terminar de inicializar
        yield return LocalizationSettings.InitializationOperation;

        var opcoes = new List<string>();
        var locales = LocalizationSettings.AvailableLocales.Locales;

        foreach (var locale in locales)
            opcoes.Add(locale.LocaleName); // ex: "Portuguese (Brazil)", "English"

        dropdown.ClearOptions();
        dropdown.AddOptions(opcoes);

        // marca no dropdown o idioma que já está ativo agora
        dropdown.value = locales.IndexOf(LocalizationSettings.SelectedLocale);
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.AddListener(TrocarIdioma);
    }

    void TrocarIdioma(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}