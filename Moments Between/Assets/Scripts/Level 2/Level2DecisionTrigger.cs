// Level2DecisionTrigger.cs (angepasst für TextMeshProUGUI)
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level2DecisionTrigger : MonoBehaviour
{
    [Header("UI References")]
    public GameObject decisionUI;
    public Button buttonLeft;
    public Button buttonRight;
    public TextMeshProUGUI questionText;

    [Header("Decision Settings")]
    public float slowTimeScale = 0.5f;
    public string levelID = "Level2";
    public string hubSceneName;

    private bool decisionActive = false;

    void Start()
    {
        decisionUI.SetActive(false);
        buttonLeft.onClick.AddListener(() => OnDecision(false));  // Links = nicht erzählen
        buttonRight.onClick.AddListener(() => OnDecision(true)); // Rechts = erzählen
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = 0.02f * slowTimeScale;
        decisionUI.SetActive(true);
        decisionActive = true;
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
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // Entscheidung speichern
        FlashbackChoice choice = choseRight 
            ? FlashbackChoice.ChoseRight 
            : FlashbackChoice.None;
        GameManager.Instance.SetChoice(levelID, choice);

        // Zurück in die Hub
        SceneTransitionManager.Instance.LoadHub(hubSceneName);
        Destroy(gameObject);
    }
}