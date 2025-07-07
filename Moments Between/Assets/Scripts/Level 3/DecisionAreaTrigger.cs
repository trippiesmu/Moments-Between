// DecisionAreaTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class DecisionAreaTrigger : MonoBehaviour
{
    [Header("Level & Hub")]
    public string levelID = "Level3";
    public string hubSceneName = "HubScene";

    [Header("Choice")]
    [Tooltip("True = Bett A (ChoseLeft), False = Bett B (ChoseRight)")]
    public bool isBedA;

    [Header("Optionaler Marker")]
    public GameObject highlightObject;

    private Collider col;

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
        IntermediateTrigger.OnDecisionPhaseReady += EnableArea;
    }

    void OnDisable()
    {
        IntermediateTrigger.OnDecisionPhaseReady -= EnableArea;
    }

    private void EnableArea()
    {
        col.enabled = true;
        if (highlightObject) highlightObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!col.enabled || !other.CompareTag("Player")) return;

        // Mapping: Bett A = ChoseLeft, Bett B = ChoseRight
        var choice = isBedA 
            ? FlashbackChoice.ChoseLeft 
            : FlashbackChoice.ChoseRight;
        GameManager.Instance.SetChoice(levelID, choice);

        // zurück in die Hub
        if (SceneTransitionManager.Instance != null)
            SceneTransitionManager.Instance.ReturnToHub(hubSceneName);
        else
            SceneManager.LoadScene(hubSceneName);

        // Einmal-Trigger
        col.enabled = false;
        if (highlightObject) highlightObject.SetActive(false);
        enabled = false;
    }
}
