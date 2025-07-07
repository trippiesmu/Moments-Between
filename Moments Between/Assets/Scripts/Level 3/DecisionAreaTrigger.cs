// DecisionAreaTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class DecisionAreaTrigger : MonoBehaviour
{
    [Header("Level & Hub")]
    [Tooltip("ID für GameManager.SetChoice")]
    public string levelID = "Level3";
    [Tooltip("Exakter Name der Hub-Szene")]
    public string hubSceneName = "HubScene";

    [Header("Choice")]
    [Tooltip("True = Bett A (None), False = Bett B (ChoseRight)")]
    public bool isBedA;

    [Header("Optionaler Marker")]
    public GameObject highlightObject;

    Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
        if (col == null) Debug.LogError($"{name}: Collider fehlt!");
    }

    void Start()
    {
        col.enabled = false;
        if (highlightObject) highlightObject.SetActive(false);
    }

    void OnEnable()
    {
        // nur aufs statische Event hören – kein Level3Manager mehr hier
        IntermediateTrigger.OnDecisionPhaseReady += EnableArea;
    }

    void OnDisable()
    {
        IntermediateTrigger.OnDecisionPhaseReady -= EnableArea;
    }

    void EnableArea()
    {
        col.enabled = true;
        if (highlightObject) highlightObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!col.enabled || !other.CompareTag("Player")) return;

        // Entscheidung speichern
        var choice = isBedA
            ? FlashbackChoice.None
            : FlashbackChoice.ChoseRight;
        GameManager.Instance.SetChoice(levelID, choice);

        // genau wie in den anderen Levels: zurück in die Hub
        if (SceneTransitionManager.Instance != null)
            SceneTransitionManager.Instance.ReturnToHub(hubSceneName);
        else
        {
            Debug.LogError($"DecisionAreaTrigger: Kein SceneTransitionManager, lade Hub direkt.");
            SceneManager.LoadScene(hubSceneName);
        }

        // Einmal-Trigger
        col.enabled = false;
        if (highlightObject) highlightObject.SetActive(false);
        enabled = false;
    }
}
