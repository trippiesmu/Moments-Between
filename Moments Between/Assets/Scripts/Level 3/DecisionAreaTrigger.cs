// DecisionAreaTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class DecisionAreaTrigger : MonoBehaviour
{
    [Header("Decision Settings")]
    [Tooltip("True = Bett A (None), False = Bett B (ChoseRight)")]
    public bool isBedA;
    public string levelID = "Level3";
    public string hubSceneName = "HubScene";

    [Header("Visuals (optional)")]
    public MeshRenderer highlightRenderer;

    private Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        // Collider und Visual zunächst deaktivieren
        col.enabled = false;
        if (highlightRenderer) highlightRenderer.enabled = false;

        // sobald Manager ready ist, Collider öffnen
        Level3Manager.Instance.OnReadyToDecide += EnableArea;
    }

    void OnDestroy()
    {
        if (Level3Manager.Instance != null)
            Level3Manager.Instance.OnReadyToDecide -= EnableArea;
    }

    void EnableArea()
    {
        col.enabled = true;
        if (highlightRenderer) highlightRenderer.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!col.enabled || !other.CompareTag("Player")) return;

        // Entscheidung speichern
        var choice = isBedA
            ? FlashbackChoice.None
            : FlashbackChoice.ChoseRight;
        GameManager.Instance.SetChoice(levelID, choice);

        // Zur Hub zurück
        SceneTransitionManager.Instance.ReturnToHub(hubSceneName);
    }
}