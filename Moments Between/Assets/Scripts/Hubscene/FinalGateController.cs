// FinalGateController.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class FinalGateController : MonoBehaviour
{
    public static FinalGateController Instance { get; private set; }

    [Header("Anzahl Pflicht-Levels")]
    public int requiredChoices = 3;

    [Header("Effekt-Prefab")]
    public GameObject effectPrefab;
    public Vector3 effectOffset = Vector3.up * 1.5f;

    [Header("Szenennamen")]
    public string hubSceneName = "HubScene";
    public string endSceneName = "EndScene";

    Collider col;
    GameObject effectInstance;
    bool gateEnabled;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        col = GetComponent<Collider>();
        col.isTrigger = true;
        col.enabled = false;

        GameManager.Instance.OnChoiceChanged += OnChoiceChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;

        TryEnableGate();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnChoiceChanged -= OnChoiceChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (Instance == this) Instance = null;
    }

    void OnChoiceChanged(string lvl, FlashbackChoice c) => TryEnableGate();
    void OnSceneLoaded(Scene s, LoadSceneMode m)    => TryEnableGate();

    void TryEnableGate()
    {
        if (SceneManager.GetActiveScene().name != hubSceneName) return;
        if (GameManager.Instance.ChoiceCount < requiredChoices) return;
        if (gateEnabled) return;

        gateEnabled  = true;
        col.enabled  = true;
        SpawnEffect();
    }

    void SpawnEffect()
    {
        if (effectPrefab == null || effectInstance != null) return;
        Vector3 pos = transform.position + effectOffset;
        effectInstance = Instantiate(effectPrefab, pos, Quaternion.identity, transform);
        effectInstance.SetActive(true);
    }

    /// <summary>Wird von GateInteractionTrigger auf E gedrückt.</summary>
    public void Interact()
    {
        if (!gateEnabled) return;
        if (effectInstance != null) Destroy(effectInstance);

        if (SceneTransitionManager.Instance != null)
            SceneTransitionManager.Instance.LoadFlashback(endSceneName);
        else
            SceneManager.LoadScene(endSceneName);
    }
}
