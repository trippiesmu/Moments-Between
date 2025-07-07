// PrelogueController.cs
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PrelogueController : MonoBehaviour
{
    [Header("Dialogue Lines")]
    [TextArea]
    public string[] lines;

    [Header("UI References")]
    [Tooltip("Panel mit dem Dialogfenster (Canvas)")]
    public GameObject dialogueUI;
    [Tooltip("TextMeshProUGUI im Dialogfenster")]
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    [Header("Hub Scene Name")]
    [Tooltip("Name der Hub-Szene, exakt aus Build Settings")]
    public string hubSceneName = "HubScene";

    private int idx;
    private bool isTyping;
    private bool dialogueFinished;

    void Start()
    {
        if (dialogueUI != null) dialogueUI.SetActive(true);
        idx = 0;
        StartCoroutine(TypeLine(lines[idx]));
    }

    void Update()
    {
        // wenn noch im Tippen, nichts tun
        if (isTyping) return;

        // solange Dialog nicht fertig, blättere Zeilen
        if (!dialogueFinished)
        {
            if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                idx++;
                if (idx < lines.Length)
                {
                    StartCoroutine(TypeLine(lines[idx]));
                }
                else
                {
                    // Letzte Zeile durch – markiere Ende
                    dialogueFinished = true;
                    // Option: UI weiter sichtbar lassen oder sofort ausblenden
                    dialogueUI.SetActive(false);
                }
            }
        }
        else
        {
            // Dialog ist fertig: Klick/Taste → Hub laden
            if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                if (SceneTransitionManager.Instance != null)
                    SceneTransitionManager.Instance.ReturnToHub(hubSceneName);
                else
                    SceneManager.LoadScene(hubSceneName);
            }
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
}
