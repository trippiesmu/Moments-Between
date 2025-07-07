using UnityEngine;
using TMPro;
using System.Collections;

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
    [Tooltip("Exakter Name der Flashback-Szene, wie in den Build Settings")]
    public string flashbackSceneName;

    private int idx;
    private bool isTyping;

    void Start()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(false);
    }

    public void TriggerDialogue()
    {
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
            return;
        }

        idx++;
        if (idx < lines.Length)
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
        foreach (var c in line)
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

        // Debug-Ausgabe, damit du siehst, was geladen wird:
        Debug.Log($"[DialogueTrigger] Loading flashback scene: '{flashbackSceneName}'");

        // Hier genau den Inspector-String nutzen:
        if (SceneTransitionManager.Instance != null)
            SceneTransitionManager.Instance.LoadFlashback(flashbackSceneName);
        else
            Debug.LogError("No SceneTransitionManager instance found!");
    }
}
