// PatientDialogueTrigger.cs
using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class PatientDialogueTrigger : MonoBehaviour
{
    [Header("Patient Settings")]
    [Tooltip("ID: 'A' oder 'B'")]
    public string patientID;
    public string[] lines;

    [Header("UI")]
    public GameObject dialogueUI;           // Panel mit TMP-Text
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    private int idx;
    private bool isTyping, active;

    void Start()
    {
        dialogueUI?.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (active || !other.CompareTag("Player")) return;
        active = true;
        idx = 0;
        dialogueUI.SetActive(true);
        DialogueTrigger.dialogueActive = true;  // block Movement/Prompt
        StartCoroutine(TypeLine(lines[idx]));
    }

    void Update()
    {
        if (!active) return;
        if (Input.GetKeyDown(KeyCode.E))
            Advance();
    }

    void Advance()
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
        dialogueUI.SetActive(false);
        DialogueTrigger.dialogueActive = false;
        // Manager benachrichtigen
        Level3Manager.Instance.RegisterDialogue(patientID);
        Destroy(gameObject);
    }
}