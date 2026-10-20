using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;

public class EfeitoDigitacao : MonoBehaviour
{
    public Text campoDeTexto;
    public float velocidade = 0.03f;

    // Referência para a chave da String Table (escolha tabela e chave no Inspector)
    public LocalizedString textoParaDigitar;

    void OnEnable()
    {
        // Se o jogador trocar o idioma com o objeto na tela, digita de novo
        LocalizationSettings.SelectedLocaleChanged += AoTrocarIdioma;
        Reiniciar();
    }

    void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= AoTrocarIdioma;
        StopAllCoroutines();
    }

    void AoTrocarIdioma(Locale novoIdioma)
    {
        Reiniciar();
    }

    void Reiniciar()
    {
        if (campoDeTexto == null)
        {
            Debug.LogWarning("EfeitoDigitacao: 'campoDeTexto' não está ligado no Inspector.", this);
            return;
        }

        StopAllCoroutines();
        StartCoroutine(Digitar());
    }

    IEnumerator Digitar()
    {
        campoDeTexto.text = "";

        // Espera o sistema de Localization terminar de iniciar
        yield return LocalizationSettings.InitializationOperation;

        if (textoParaDigitar == null || textoParaDigitar.IsEmpty)
        {
            Debug.LogWarning("EfeitoDigitacao: nenhuma tabela/chave foi escolhida em 'Texto Para Digitar'.", this);
            yield break;
        }

        // Busca o texto traduzido na tabela, no idioma atual
        var operacao = textoParaDigitar.GetLocalizedStringAsync();
        yield return operacao;
        string texto = operacao.Result;

        if (string.IsNullOrEmpty(texto))
        {
            Debug.LogWarning("EfeitoDigitacao: a chave existe, mas o texto está vazio nesse idioma.", this);
            yield break;
        }

        yield return new WaitForSeconds(0.1f); // Pequeno atraso para evitar bugs visuais

        foreach (char letra in texto)
        {
            campoDeTexto.text += letra;
            yield return new WaitForSeconds(velocidade);
        }
    }
}