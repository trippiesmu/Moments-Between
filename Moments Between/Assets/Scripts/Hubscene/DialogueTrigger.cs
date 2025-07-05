// DialogueTrigger.cs
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
        SceneTransitionManager.Instance.LoadFlashback(flashbackSceneName);
    }
}
