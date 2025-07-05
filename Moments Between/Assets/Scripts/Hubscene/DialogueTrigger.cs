// DialogueTrigger.cs
using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Wird an jedes Hub-Objekt gehängt, das per Dialog in ein Level führt.
/// Deaktiviert sich selbst, sobald das Level bereits einmal gestartet wurde.
/// </summary>
[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    public static bool dialogueActive = false;

    [Header("Dialogue Lines")]
    [TextArea] public string[] lines;

    [Header("UI References")]
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    [Header("Scene Configuration")]
    [Tooltip("Name der Flashback-Szene, exakt wie in Build Settings.")]
    public string flashbackSceneName;

    private int idx;
    private bool isTyping;

    void Start()
    {
        // Wenn dieses Flashback schon gespielt wurde, komplett deaktivieren
        if (GameManager.Instance.HasChoice(flashbackSceneName))
        {
            GetComponent<Collider>().enabled = false;
            enabled = false;
            return;
        }

        if (dialogueUI != null)
            dialogueUI.SetActive(false);
    }

    /// <summary>Wird vom InteractionSystem gerufen, wenn der Spieler E drückt.</summary>
    public void TriggerDialogue()
    {
        if (GameManager.Instance.HasChoice(flashbackSceneName))
            return; // doppelte Auslösung verhindern

        dialogueActive = true;
        idx = 0;
        dialogueUI?.SetActive(true);
        StartCoroutine(TypeLine(lines[idx]));
    }

    void Update()
    {
        if (!dialogueActive) return;
        if (Input.GetKeyDown(KeyCode.E))
            Advance();
    }

    private void Advance()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = lines[idx];
            isTyping = false;
        }
        else if (++idx < lines.Length)
        {
            StartCoroutine(TypeLine(lines[idx]));
        }
        else
        {
            EndDialogueAndLoad();
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

    private void EndDialogueAndLoad()
    {
        dialogueUI?.SetActive(false);
        dialogueActive = false;

        // Markiere als gespielt (None, da hier keine echte Choice)
        GameManager.Instance.SetChoice(flashbackSceneName, FlashbackChoice.None);

        // Lade das Flashback-Level
        SceneTransitionManager.Instance.LoadFlashback(flashbackSceneName);
    }
}
