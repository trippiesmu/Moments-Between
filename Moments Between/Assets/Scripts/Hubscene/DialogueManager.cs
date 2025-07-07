using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI-Prefab (einmalige Zuweisung)")]
    [Tooltip("Ziehe hier dein Dialogue-Canvas-Prefab hinein")]
    public GameObject dialogueUIPrefab;

    private GameObject dialogueUI;           
    private TextMeshProUGUI dialogueText;     
    public float typingSpeed = 0.05f;

    private Queue<string> linesQueue;
    private string targetScene;
    private bool IsTyping;

    public bool DialogueActive { get; private set; }

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Einmalig das UI-Prefab instanziieren und persistent halten
        if (dialogueUIPrefab != null)
        {
            dialogueUI = Instantiate(dialogueUIPrefab);
            DontDestroyOnLoad(dialogueUI);
            dialogueText = dialogueUI.GetComponentInChildren<TextMeshProUGUI>();
            dialogueUI.SetActive(false);
        }
        else
        {
            Debug.LogError("DialogueManager: dialogueUIPrefab nicht gesetzt!");
        }

        // Reset bei jedem Szenenwechsel
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        // Falls du das UI nur in der Hub sehen willst, kannst du hier filtern.
        // Für Flashbacks brauchen wir UI aber auch, deshalb lassen wir es an.
        DialogueActive = false;
        if (dialogueUI != null) dialogueUI.SetActive(false);
    }

    void Update()
    {
        if (DialogueActive && !IsTyping && Input.GetKeyDown(KeyCode.E))
            DisplayNextLine();
    }

    public void StartDialogue(string[] lines, string flashbackSceneName)
    {
        if (dialogueUI == null || dialogueText == null) return;

        DialogueActive = true;
        targetScene = flashbackSceneName;
        linesQueue = new Queue<string>(lines);

        dialogueUI.SetActive(true);
        DisplayNextLine();
    }

    private void DisplayNextLine()
    {
        if (linesQueue.Count == 0)
        {
            EndDialogueAndLoad();
            return;
        }

        StopAllCoroutines();
        StartCoroutine(TypeLine(linesQueue.Dequeue()));
    }

    private IEnumerator TypeLine(string line)
    {
        IsTyping = true;
        dialogueText.text = "";
        foreach (var c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        IsTyping = false;
    }

    private void EndDialogueAndLoad()
    {
        DialogueActive = false;
        dialogueUI.SetActive(false);

        Debug.Log($"[DialogueManager] Loading flashback scene: '{targetScene}'");
        if (SceneTransitionManager.Instance != null)
            SceneTransitionManager.Instance.LoadFlashback(targetScene);
        else
            Debug.LogError("DialogueManager: Kein SceneTransitionManager gefunden!");
    }
}
