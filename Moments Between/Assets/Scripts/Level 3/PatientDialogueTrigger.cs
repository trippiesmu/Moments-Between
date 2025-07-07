// PatientDialogueTrigger.cs
using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class PatientDialogueTrigger : MonoBehaviour
{
    [Header("Patienten-ID: 'A' oder 'B'")]
    public string patientID;
    [TextArea] public string[] lines;

    [Header("UI References")]
    public GameObject promptUI;
    public TextMeshProUGUI promptText;
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    bool playerInRange, active, isTyping;
    int idx;

    void Start()
    {
        promptUI?.SetActive(false);
        dialogueUI?.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (active || !other.CompareTag("Player")) return;
        playerInRange = true;
        promptUI?.SetActive(true);
        promptText.text = "Press E to talk";
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        promptUI?.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !active && Input.GetKeyDown(KeyCode.E))
        {
            promptUI?.SetActive(false);
            BeginDialogue();
        }
        else if (active && Input.GetKeyDown(KeyCode.E))
        {
            AdvanceDialogue();
        }
    }

    void BeginDialogue()
    {
        active = true;
        idx = 0;
        dialogueUI?.SetActive(true);
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
            StartCoroutine(TypeLine(lines[idx]));
        else
            EndDialogue();
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
        dialogueUI?.SetActive(false);
        active = false;

        if (Level3Manager.Instance != null)
            Level3Manager.Instance.RegisterDialogue(patientID);
        else
            Debug.LogError("PatientDialogueTrigger: Kein Level3Manager in Szene!");

        GetComponent<Collider>().enabled = false;
        enabled = false;
    }
}
