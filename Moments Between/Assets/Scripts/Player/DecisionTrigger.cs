using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Stoppt das Auto an einem Triggerpunkt, zeigt eine UI für Links/Rechts-Entscheidung,
/// und führt danach verschiedene Aktionen aus.
/// </summary>
[RequireComponent(typeof(Collider))]
public class DecisionTrigger : MonoBehaviour
{
    [Header("References")]
    public CarController carController;
    public GameObject decisionUI;          // Canvas-Panel mit Buttons
    public Button buttonLeft;              // Button für Links
    public Button buttonRight;             // Button für Rechts

    [Header("Steering")]
    public float steerAngle = 30f;         // Drehwinkel bei Entscheidung

    [Header("Events")]
    public UnityEvent onChooseLeft;        // Inspector: Aktionen bei Links
    public UnityEvent onChooseRight;       // Inspector: Aktionen bei Rechts

    private bool decisionActive = false;

    void Start()
    {
        // UI ausblenden und Button-Listener setzen
        decisionUI.SetActive(false);
        buttonLeft.onClick.AddListener(ChooseLeft);
        buttonRight.onClick.AddListener(ChooseRight);
    }

    void OnTriggerEnter(Collider other)
    {
        // Nur der Player (Tag "Player") löst die Entscheidung aus
        if (other.CompareTag("Player"))
        {
            carController.canDrive = false;
            decisionUI.SetActive(true);
            decisionActive = true;
        }
    }

    void Update()
    {
        if (!decisionActive) return;

        // Tasteneingaben abfragen
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            ChooseLeft();
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            ChooseRight();
    }

    private void ChooseLeft()
    {
        decisionActive = false;
        decisionUI.SetActive(false);

        // Lenken und Events auslösen
        carController.SteerLeft(steerAngle);
        onChooseLeft?.Invoke();

        // Trigger nur einmal verwenden
        Destroy(gameObject);
    }

    private void ChooseRight()
    {
        decisionActive = false;
        decisionUI.SetActive(false);

        carController.SteerRight(steerAngle);
        onChooseRight?.Invoke();

        Destroy(gameObject);
    }
}
