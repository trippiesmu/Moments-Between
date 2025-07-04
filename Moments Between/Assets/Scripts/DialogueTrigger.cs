// DialogueTrigger.cs
// Steuert Dialog mit Typewriter-Effekt und lädt danach direkt die konfigurierte Flashback-Szene.
using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Lines")]
    [TextArea] public string[] lines;

    [Header("UI References")]
    public GameObject dialogueUI;             // Panel mit TMP-Text + Next-Hinweis
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    [Header("Scene Configuration")]
    [Tooltip("Name der Flashback-Szene, exakt so wie in Build Settings")]
    public string flashbackSceneName;

    private int idx;
    private bool isTyping;
    private bool active;

    void Start()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(false);
    }

    void Update()
    {
        if (!active) return;
        if (Input.GetKeyDown(KeyCode.E))
            Advance();
    }

    /// <summary>
    /// Wird vom InteractionSystem aufgerufen, wenn E gedrückt wurde.
    /// </summary>
    public void TriggerDialogue()
    {
        if (lines == null || lines.Length == 0)
        {
            LoadFlashback();
            return;
        }
        idx = 0;
        active = true;
        dialogueUI?.SetActive(true);
        StartCoroutine(TypeLine(lines[idx]));
    }

    private void Advance()
    {
        if (isTyping)
        {
            // Tippeffekt abbrechen und komplette Zeile anzeigen
            StopAllCoroutines();
            dialogueText.text = lines[idx];
            isTyping = false;
        }
        else
        {
            idx++;
            if (idx < lines.Length)
                StartCoroutine(TypeLine(lines[idx]));
            else
                LoadFlashback();
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

    private void LoadFlashback()
    {
        // Dialog-UI ausblenden
        dialogueUI?.SetActive(false);
        active = false;

        // Flashback-Level laden
        if (!string.IsNullOrWhiteSpace(flashbackSceneName))
            SceneTransitionManager.Instance.LoadFlashback(flashbackSceneName);
    }
}
