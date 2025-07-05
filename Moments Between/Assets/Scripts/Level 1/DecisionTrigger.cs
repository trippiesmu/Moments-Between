// DecisionTrigger.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Collider))]
public class DecisionTrigger : MonoBehaviour
{
    [Header("References")]
    public CarController playerCar;
    public FollowerCarController followerCar;

    [Header("Decision UI")]
    public GameObject decisionUI;
    public Button buttonStraight;
    public Button buttonRight;
    public TextMeshProUGUI timerText;

    [Header("Decision Deadline")]
    public Transform decisionDeadlinePoint;

    [Header("Settings")]
    public float steerAngle = 30f;
    public float slowTimeScale = 0.5f;
    public float slowSpeedFactor = 0.5f;
    public string hubSceneName;

    [Header("Level Info")]
    [Tooltip("Eindeutiger Key für dieses Flashback-Level, z.B. 'Level1'")]
    public string levelID;

    private bool decisionActive = false;
    private float originalSpeed;
    private float remainingTime;

    void Start()
    {
        if (playerCar != null)
            originalSpeed = playerCar.speed;

        decisionUI.SetActive(false);
        buttonStraight.onClick.AddListener(() => Choose(false));
        buttonRight.onClick.AddListener(() => Choose(true));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !decisionActive)
        {
            decisionActive = true;

            // Zeitlupe und langsam fahren
            Time.timeScale = slowTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            if (playerCar != null)
                playerCar.speed = originalSpeed * slowSpeedFactor;

            // Follower startet sofort
            followerCar?.StartMove();

            // Berechne verbleibende Zeit bis Deadline
            if (decisionDeadlinePoint != null && playerCar != null)
            {
                float dist = Vector3.Distance(playerCar.transform.position, decisionDeadlinePoint.position);
                remainingTime = dist / playerCar.speed;
            }

            // UI aktivieren
            decisionUI.SetActive(true);
        }
    }

    void Update()
    {
        if (!decisionActive)
            return;

        // Countdown-Update
        if (remainingTime > 0f)
        {
            remainingTime -= Time.unscaledDeltaTime;
            if (timerText != null)
                timerText.text = Mathf.Max(0f, remainingTime).ToString("F1") + "s";

            if (remainingTime <= 0f)
            {
                Choose(false);
                return;
            }
        }

        // Tastatur-Shortcuts
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Choose(false);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Choose(true);
    }

    private void Choose(bool turnRight)
    {
        decisionActive = false;
        decisionUI.SetActive(false);

        // Entscheidung an GameManager senden
        if (!string.IsNullOrEmpty(levelID))
        {
            var choice = turnRight ? FlashbackChoice.ChoseRight : FlashbackChoice.None;
            GameManager.Instance.SetChoice(levelID, choice);
        }

        // Optionale Lenkung
        if (turnRight)
            playerCar?.SteerRight(steerAngle);

        // Rückkehr-Koroutine starten
        StartCoroutine(ReturnAfterDelay());
    }

    private IEnumerator ReturnAfterDelay()
    {
        // Warte real 1 Sekunde
        yield return new WaitForSecondsRealtime(1f);

        // Zeitnormalisierung
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        if (playerCar != null)
            playerCar.speed = originalSpeed;

        // Hub-Szene laden
        if (!string.IsNullOrWhiteSpace(hubSceneName))
            SceneManager.LoadScene(hubSceneName);

        // Skript-GameObject kann zerstört werden
        Destroy(gameObject);
    }
}

