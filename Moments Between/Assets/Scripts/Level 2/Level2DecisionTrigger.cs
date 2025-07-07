// Level2DecisionTrigger.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Collider))]
public class Level2DecisionTrigger : MonoBehaviour
{
    [Header("UI References")]
    public GameObject decisionUI;
    public Button buttonLeft;   // Erzähl’s dem Boss = ChoseLeft
    public Button buttonRight;  // Schweig = ChoseRight
    public TextMeshProUGUI questionText;

    [Header("Settings")]
    public float slowTimeScale = 0.5f;
    public string levelID = "Level2";
    public string hubSceneName;

    private bool decisionActive;

    void Start()
    {
        if (decisionUI != null) decisionUI.SetActive(false);
        buttonLeft.onClick.AddListener(() => OnDecision(false));
        buttonRight.onClick.AddListener(() => OnDecision(true));
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        decisionActive = true;
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = 0.02f * slowTimeScale;
        decisionUI.SetActive(true);
    }

    void Update()
    {
        if (!decisionActive) return;
        if (Input.GetKeyDown(KeyCode.LeftArrow))  OnDecision(false);
        if (Input.GetKeyDown(KeyCode.RightArrow)) OnDecision(true);
    }

    private void OnDecision(bool choseRight)
    {
        decisionActive = false;
        decisionUI.SetActive(false);

        // Zeitlupe zurücksetzen
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // Mapping: false = ChoseLeft, true = ChoseRight
        var choice = choseRight 
            ? FlashbackChoice.ChoseRight 
            : FlashbackChoice.ChoseLeft;
        GameManager.Instance.SetChoice(levelID, choice);

        // zurück in die Hub
        if (SceneTransitionManager.Instance != null)
            SceneTransitionManager.Instance.ReturnToHub(hubSceneName);
        else
            Debug.LogError("Kein SceneTransitionManager gefunden.");

        // Trigger deaktivieren
        GetComponent<Collider>().enabled = false;
        enabled = false;
    }
}
