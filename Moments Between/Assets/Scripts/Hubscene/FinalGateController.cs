// FinalGateController.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(Collider))]
public class FinalGateController : MonoBehaviour
{
    public static FinalGateController Instance { get; private set; }

    [Header("Zu prüfende Level")]
    [Tooltip("Exakte Szenennamen deiner Flashback-Levels")]
    public string[] levelIDs = { "Level1", "Level2", "Level3" };

    [Header("Effekt-Prefab")]
    [Tooltip("Prefab mit Partikeln/Outline o.ä.")]
    public GameObject effectPrefab;
    [Tooltip("Offset relativ zur Position dieses GameObjects")]
    public Vector3 effectOffset = Vector3.up * 1.5f;

    [Header("Szenennamen")]
    [Tooltip("Name der Hub-Szene, exakt wie in Build Settings")]
    public string hubSceneName = "HubScene";
    [Tooltip("Name der End-Szene, die geladen werden soll")]
    public string endSceneName = "EndScene";

    Collider col;
    GameObject effectInstance;
    bool gateEnabled;

    void Awake()
    {
        // Singleton + Persistenz
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        col = GetComponent<Collider>();
        col.isTrigger = true;
        col.enabled = false;

        // Auf Choice-Änderungen und Szene-Wechsel hören
        GameManager.Instance.OnChoiceChanged += OnChoiceChanged;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
        if (GameManager.Instance != null)
            GameManager.Instance.OnChoiceChanged -= OnChoiceChanged;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Wird nach jeder SetChoice aufgerufen
    void OnChoiceChanged(string levelID, FlashbackChoice choice)
        => TryEnableGate();

    // Bei jedem Szenenwechsel prüfen
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        => TryEnableGate();

    void TryEnableGate()
    {
        // Nur in der Hub-Szene
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != hubSceneName)
            return;

        // Alle drei Levels gespielt?
        foreach (var id in levelIDs)
            if (!GameManager.Instance.HasChoice(id))
                return;

        // Einmal freischalten
        if (!gateEnabled)
        {
            gateEnabled = true;
            col.enabled = true;
            SpawnEffect();
        }
    }

    void SpawnEffect()
    {
        if (effectPrefab == null || effectInstance != null) return;
        var pos = transform.position + effectOffset;
        effectInstance = Instantiate(effectPrefab, pos, Quaternion.identity, transform);
        effectInstance.SetActive(true);
    }

    /// <summary>
    /// Wird von GateInteractionTrigger aufgerufen, wenn der Spieler E drückt.
    /// </summary>
    public void Interact()
    {
        if (!gateEnabled) return;

        // Optional vorher Effekt aufräumen
        if (effectInstance != null)
            Destroy(effectInstance);

        // Endszene laden
        if (SceneTransitionManager.Instance != null)
            SceneTransitionManager.Instance.LoadFlashback(endSceneName);
        else
            SceneManager.LoadScene(endSceneName);
    }
}
