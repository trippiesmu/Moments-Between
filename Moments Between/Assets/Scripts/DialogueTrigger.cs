// DialogueTrigger.cs
// Steuert eine Dialogsequenz und ruft nach Abschluss das Flashback-Level auf.
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [TextArea] public string[] dialogueLines;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueUI;
    public float typingSpeed = 0.05f;

    [Header("On Dialogue Complete")]
    public UnityEvent onDialogueComplete;  // Hier StartFlashback() von FlashbackDecisionTrigger eintragen

    private int index;
    private bool isTyping;

    public void TriggerDialogue()
    {
        dialogueUI.SetActive(true);
        index = 0;
        NextLine();
    }

    public void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = dialogueLines[index - 1];
            isTyping = false;
            return;
        }

        if (index < dialogueLines.Length)
        {
            StartCoroutine(TypeLine(dialogueLines[index]));
            index++;
        }
        else
        {
            dialogueUI.SetActive(false);
            onDialogueComplete?.Invoke();
        }
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = string.Empty;
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}