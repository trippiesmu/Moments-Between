// FinalGateController.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class FinalGateController : MonoBehaviour
{
    [Header("Zu prüfende Level")]
    [Tooltip("Szenennamen der Flashback-Levels")]
    public string[] levelIDs = { "Level1", "Level2", "Level3" };

    [Header("Visueller Effekt")]
    [Tooltip("Prefab oder Kind-Object, z.B. leuchtender Rahmen an der Tür")]
    public GameObject effectObject;

    [Header("Szenennamen")]
    [Tooltip("Name der Hub-Szene, exakt wie in Build Settings")]
    public string hubSceneName = "HubScene";
    [Tooltip("Name der Endszene, die nach allen 3 Levels geladen wird")]
    public string endSceneName = "EndScene";

    private Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
        // Collider & Effekt initial deaktivieren
        col.enabled = false;
        if (effectObject != null)
            effectObject.SetActive(false);
    }

    void Start()
    {
        // Nur in der Hub-Szene prüfen
        if (SceneManager.GetActiveScene().name == hubSceneName
            && AllLevelsPlayed())
        {
            col.enabled = true;
            if (effectObject != null)
                effectObject.SetActive(true);
        }
    }

    private bool AllLevelsPlayed()
    {
        foreach (var id in levelIDs)
        {
            if (!GameManager.Instance.HasChoice(id))
                return false;
        }
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!col.enabled || !other.CompareTag("Player"))
            return;

        // Lade die Endszene
        if (SceneTransitionManager.Instance != null)
            SceneTransitionManager.Instance.LoadFlashback(endSceneName);
        else
            SceneManager.LoadScene(endSceneName);
    }
}