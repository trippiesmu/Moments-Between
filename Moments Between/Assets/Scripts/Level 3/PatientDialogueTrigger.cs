// PatientDialogueTrigger.cs
// Dialog-Script bleibt auf dem Character-Mesh, entfernt kein GameObject mehr
using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class PatientDialogueTrigger : MonoBehaviour
{
    [Header("Patient Settings")]
    public string patientID;              // "A" oder "B"
    [TextArea] public string[] lines;     // Kurze Sätze (4–5 pro Patient)

    [Header("UI References")]
    [Tooltip("Panel mit 'Press E to talk'-Hinweis")]
    public GameObject promptUI;
    [Tooltip("Text im Prompt-Panel")]
    public TextMeshProUGUI promptText;
    [Tooltip("Panel mit Dialog-Text")]
    public GameObject dialogueUI;
    [Tooltip("Text im Dialog-Panel")]
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    bool playerInRange = false;
    bool active = false;
    int idx = 0;
    bool isTyping = false;

    void Start()
    {
        if (promptUI    != null) promptUI   .SetActive(false);
        if (dialogueUI  != null) dialogueUI .SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!active && other.CompareTag("Player"))
        {
            playerInRange = true;
            if (promptUI != null)
            {
                promptUI.SetActive(true);
                promptText.text = "Press E to talk";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    void Update()
    {
        // Starts dialog
        if (playerInRange && !active && Input.GetKeyDown(KeyCode.E))
        {
            if (promptUI != null) promptUI.SetActive(false);
            BeginDialogue();
        }

        // Continues dialog
        if (active && Input.GetKeyDown(KeyCode.E))
            AdvanceDialogue();
    }

    void BeginDialogue()
    {
        active = true;
        DialogueTrigger.dialogueActive = true; // block Movement + Prompt
        idx = 0;
        if (dialogueUI != null) dialogueUI.SetActive(true);
        StartCoroutine(TypeLine(lines[idx]));
    }

    void AdvanceDialogue()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = lines[idx];
            isTyping = false;
            return;
        }

        idx++;
        if (idx < lines.Length)
        {
            StartCoroutine(TypeLine(lines[idx]));
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine(string line)
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

    void EndDialogue()
    {
        if (dialogueUI != null) dialogueUI.SetActive(false);
        DialogueTrigger.dialogueActive = false;
        active = false;

        // Registriere beim Manager
        Level3Manager.Instance.RegisterDialogue(patientID);

        // Collider und dieses Script deaktivieren, aber Mesh bleibt erhalten
        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        this.enabled = false;
    }
}
