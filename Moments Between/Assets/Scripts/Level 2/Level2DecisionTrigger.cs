// Level2DecisionTrigger.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Finale Entscheidung in Level 2: Zeitlupe, Choice speichern, dann zurück in die Hub.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Level2DecisionTrigger : MonoBehaviour
{
    [Header("UI References")]
    public GameObject decisionUI;
    public Button buttonLeft;            // “Erzähl’s dem Boss” = False
    public Button buttonRight;           // “Schweig”            = True
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

        // Choice speichern (None = Links, ChoseRight = Rechts)
        var choice = choseRight 
            ? FlashbackChoice.ChoseRight 
            : FlashbackChoice.None;
        GameManager.Instance.SetChoice(levelID, choice);

        // Zurück in die Hub
        SceneTransitionManager.Instance.ReturnToHub(hubSceneName);

        // Damit der Trigger nicht erneut feuert
        GetComponent<Collider>().enabled = false;
        enabled = false;
    }
}
