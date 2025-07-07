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
    public Button buttonStraight; // Geradeaus = ChoseLeft
    public Button buttonRight;    // Rechtsabbiegen = ChoseRight
    public TextMeshProUGUI timerText;

    [Header("Decision Deadline")]
    public Transform decisionDeadlinePoint;

    [Header("Settings")]
    public float steerAngle = 30f;
    public float slowTimeScale = 0.5f;
    public float slowSpeedFactor = 0.5f;
    public string hubSceneName;

    [Header("Level Info")]
    public string levelID;

    private bool decisionActive;
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
            Time.timeScale = slowTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            if (playerCar != null)
                playerCar.speed = originalSpeed * slowSpeedFactor;
            followerCar?.StartMove();

            if (decisionDeadlinePoint != null && playerCar != null)
            {
                float dist = Vector3.Distance(playerCar.transform.position, decisionDeadlinePoint.position);
                remainingTime = dist / playerCar.speed;
            }

            decisionUI.SetActive(true);
        }
    }

    void Update()
    {
        if (!decisionActive) return;

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

        if (Input.GetKeyDown(KeyCode.UpArrow))    Choose(false);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) Choose(true);
    }

    private void Choose(bool turnRight)
    {
        decisionActive = false;
        decisionUI.SetActive(false);

        // Choice speichern: false = ChoseLeft, true = ChoseRight
        if (!string.IsNullOrEmpty(levelID))
        {
            var choice = turnRight 
                ? FlashbackChoice.ChoseRight 
                : FlashbackChoice.ChoseLeft;
            GameManager.Instance.SetChoice(levelID, choice);
        }

        if (turnRight)
            playerCar?.SteerRight(steerAngle);

        StartCoroutine(ReturnAfterDelay());
    }

    private IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        if (playerCar != null)
            playerCar.speed = originalSpeed;

        if (!string.IsNullOrWhiteSpace(hubSceneName))
            SceneManager.LoadScene(hubSceneName);

        Destroy(gameObject);
    }
}
