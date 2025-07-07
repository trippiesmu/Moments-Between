// DecisionAreaTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class DecisionAreaTrigger : MonoBehaviour
{
    [Header("Choice Settings")]
    [Tooltip("Level-ID für GameManager.SetChoice")]
    public string levelID = "Level3";
    [Tooltip("Name der Hub-Szene (exakt aus Build Settings)")]
    public string hubSceneName = "HubScene";
    [Tooltip("True = Bett A (None), False = Bett B (ChoseRight)")]
    public bool isBedA;

    [Header("Optionaler Marker")]
    [Tooltip("Ein Objekt (z.B. Plane/Icon), das zuerst unsichtbar ist")]
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
        // Vorab abschalten
        col.enabled = false;
        if (highlightObject != null)
            highlightObject.SetActive(false);
    }

    void OnEnable()
    {
        // Lausche auf den statischen Event aus IntermediateTrigger
        IntermediateTrigger.OnDecisionPhaseReady += EnableArea;
    }

    void OnDisable()
    {
        IntermediateTrigger.OnDecisionPhaseReady -= EnableArea;
    }

    /// <summary>
    /// Schaltet Collider & Marker frei, wenn IntermediateTrigger feuert.
    /// </summary>
    private void EnableArea()
    {
        col.enabled = true;
        if (highlightObject != null)
            highlightObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!col.enabled || !other.CompareTag("Player")) return;

        // Choice speichern
        var choice = isBedA ? FlashbackChoice.None : FlashbackChoice.ChoseRight;
        GameManager.Instance.SetChoice(levelID, choice);

        // Zur Hub zurück
        SceneTransitionManager.Instance.ReturnToHub(hubSceneName);

        // Nur einmal nutzbar
        col.enabled = false;
        if (highlightObject != null)
            highlightObject.SetActive(false);
        enabled = false;
    }
}
