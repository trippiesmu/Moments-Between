using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Level2StepDialogueTrigger : MonoBehaviour
{
    [Header("Quest Reference")]
    public Level2QuestLog questLog;       // Referenz auf QuestLogController

    [Header("Dialogue Settings")]
    [TextArea] public string[] dialogueLines;
    public GameObject dialogueUI;         // Panel mit TMP-Text
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    private int lineIdx;
    private bool isTyping;
    private bool active;

    void Start()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (active || !other.CompareTag("Player") ||
            (DialogueManager.Instance != null && DialogueManager.Instance.DialogueActive))
            return;

        active = true;
        lineIdx = 0;
        dialogueUI.SetActive(true);
        // block Movement + Prompt über DialogueManager.DialogueActive
        StartCoroutine(TypeLine(dialogueLines[lineIdx]));
    }


    void Update()
    {
        if (!active) return;
        if (Input.GetKeyDown(KeyCode.E))
            AdvanceDialogue();
    }

    private void AdvanceDialogue()
    {
        if (isTyping)
        {
            // sofort komplette Zeile anzeigen
            StopAllCoroutines();
            dialogueText.text = dialogueLines[lineIdx];
            isTyping = false;
            return;
        }

        lineIdx++;
        if (lineIdx < dialogueLines.Length)
        {
            StartCoroutine(TypeLine(dialogueLines[lineIdx]));
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    private void EndDialogue()
    {
        dialogueUI.SetActive(false);
        // Dialog beenden, Manager-Flag wird automatisch zurückgesetzt
        active = false;

        // Nächsten Quest-Schritt anzeigen
        questLog?.CompleteStep();
        Destroy(gameObject);
    }

}
