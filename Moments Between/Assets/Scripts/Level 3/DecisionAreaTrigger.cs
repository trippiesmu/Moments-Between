// DecisionAreaTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class DecisionAreaTrigger : MonoBehaviour
{
    [Header("Einstellungen")]
    [Tooltip("True = Bett A (None), False = Bett B (ChoseRight)")]
    public bool isBedA;
    [Tooltip("ID dieses Levels, z.B. 'Level3'")]
    public string levelID = "Level3";
    [Tooltip("Name deiner Hub-Szene exakt wie in den Build Settings")]
    public string hubSceneName = "HubScene";

    [Header("Visueller Marker")]
    [Tooltip("GameObject, das unterhalb des Bettes sichtbar wird")]
    public GameObject highlightObject;

    Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
        if (col == null)
            Debug.LogError($"[{name}] DecisionAreaTrigger braucht einen Collider!");
    }

    void Start()
    {
        // Collider & Marker zunächst deaktivieren
        if (col != null) col.enabled = false;
        if (highlightObject != null) highlightObject.SetActive(false);
        else
            Debug.LogWarning($"[{name}] highlightObject ist nicht gesetzt!");
    }

    void OnEnable()
    {
        if (Level3Manager.Instance != null)
            Level3Manager.Instance.OnReadyToDecide += EnableArea;
        else
            Debug.LogError("DecisionAreaTrigger: Level3Manager.Instance ist null!");
    }

    void OnDisable()
    {
        if (Level3Manager.Instance != null)
            Level3Manager.Instance.OnReadyToDecide -= EnableArea;
    }

    void EnableArea()
    {
        if (col != null) col.enabled = true;
        if (highlightObject != null) highlightObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        // Nur reagieren, wenn Collider aktiviert und Spieler
        if (col == null || !col.enabled) return;
        if (!other.CompareTag("Player")) return;

        Debug.Log($"[{name}] Player betritt {(isBedA ? "Bett A" : "Bett B")}");

        // Entscheidung speichern
        var choice = isBedA 
            ? FlashbackChoice.None 
            : FlashbackChoice.ChoseRight;

        if (GameManager.Instance != null)
            GameManager.Instance.SetChoice(levelID, choice);
        else
            Debug.LogError("DecisionAreaTrigger: GameManager.Instance ist null!");

        // Zur Hub zurückkehren
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.ReturnToHub(hubSceneName);
        }
        else
        {
            Debug.LogError("DecisionAreaTrigger: SceneTransitionManager.Instance ist null, lade Hub direkt.");
            SceneManager.LoadScene(hubSceneName);
        }

        // Bereich nur einmal nutzbar
        col.enabled = false;
        if (highlightObject != null) highlightObject.SetActive(false);
        enabled = false;
    }
}
