// DecisionAreaTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class DecisionAreaTrigger : MonoBehaviour
{
    [Header("Choice Settings")]
    [Tooltip("Level-ID für GameManager.SetChoice")]
    public string levelID = "Level3";
    [Tooltip("Name deiner Hub-Szene (exakt so in Build Settings)")]
    public string hubSceneName = "HubScene";
    [Tooltip("True = Bett A (None), False = Bett B (ChoseRight)")]
    public bool isBedA;

    [Header("Optionaler Visual Marker")]
    [Tooltip("Z.B. Plane oder Icon, das erst sichtbar wird, wenn der Trigger freigegeben ist")]
    public GameObject highlightObject;

    private Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
        if (col == null)
            Debug.LogError($"{name}: Collider fehlt!");
    }

    void Start()
    {
        // Collider und Marker zu Beginn ausschalten
        if (col != null) col.enabled = false;
        if (highlightObject != null) highlightObject.SetActive(false);
    }

    void OnEnable()
    {
        // Jetzt nur noch auf das statische Event aus IntermediateTrigger hören
        IntermediateTrigger.OnDecisionPhaseReady += EnableArea;
    }

    void OnDisable()
    {
        IntermediateTrigger.OnDecisionPhaseReady -= EnableArea;
    }

    private void EnableArea()
    {
        if (col != null) col.enabled = true;
        if (highlightObject != null) highlightObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        // Erst reagieren, wenn wirklich aktiviert ist und der Player reinkommt
        if (col == null || !col.enabled || !other.CompareTag("Player"))
            return;

        // Choice speichern
        if (GameManager.Instance != null)
        {
            var choice = isBedA 
                ? FlashbackChoice.None 
                : FlashbackChoice.ChoseRight;
            GameManager.Instance.SetChoice(levelID, choice);
        }
        else
        {
            Debug.LogWarning($"{name}: Kein GameManager gefunden, Choice nicht gespeichert.");
        }

        // Zur Hub zurückkehren – genau wie in Level 1 & 2
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.ReturnToHub(hubSceneName);
        }
        else
        {
            Debug.LogError($"{name}: Kein SceneTransitionManager, lade Hub direkt.");
            SceneManager.LoadScene(hubSceneName);
        }

        // Einmal-Trigger: ausschalten
        col.enabled = false;
        if (highlightObject != null) highlightObject.SetActive(false);
        enabled = false;
    }
}
