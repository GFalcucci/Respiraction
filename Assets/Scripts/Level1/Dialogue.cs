using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    [Header("Falas (cada elemento aponta para uma chave da String Table)")]
    public LocalizedString[] dialogue;

    public int dialogueIndex;

    public GameObject dialoguePanel;
    public Text dialogueText;

    public bool startDialogue = false;
    public bool readyDialogue = false;

    private bool hideDialogueCoroutineStarted = false;
    private Animator dialoguePanelAnimator;

    // fala já traduzida, escolhida quando o diálogo começa
    private string currentLine;

    void Start()
    {
        dialoguePanelAnimator = dialoguePanel.GetComponent<Animator>();
        if (dialoguePanelAnimator == null)
        {
            Debug.LogError("Componente Animator não encontrado no dialoguePanel.");
        }
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (readyDialogue)
        {
            if (!startDialogue)
            {
                StartDialogue();
            }
            else if (!hideDialogueCoroutineStarted && dialogueText.text == currentLine)
            {
                hideDialogueCoroutineStarted = true;
                StartCoroutine(HideDialogueAfterDelay(3f));
            }
        }
    }

    void StartDialogue()
    {
        startDialogue = true;
        hideDialogueCoroutineStarted = false;

        // pega o texto no idioma que está selecionado agora
        currentLine = dialogue[dialogueIndex].GetLocalizedString();

        dialoguePanel.SetActive(true);
        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        dialogueText.text = "";
        foreach (char letter in currentLine)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
    }

    IEnumerator HideDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (dialoguePanelAnimator != null)
        {
            dialoguePanelAnimator.Play("StartFadeOut");
        }

        yield return new WaitForSeconds(0.4f);
        dialoguePanel.SetActive(false);
        startDialogue = false;
        readyDialogue = false;
    }
}