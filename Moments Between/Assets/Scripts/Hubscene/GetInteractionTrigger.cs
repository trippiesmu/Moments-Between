// GateInteractionTrigger.cs
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider))]
public class GateInteractionTrigger : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Press E to enter…")]
    public GameObject promptUI;
    public TextMeshProUGUI promptText;

    bool playerInRange;

    void Start()
    {
        if (promptUI != null) promptUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // Nur anzeigen, wenn Gate auch wirklich freigeschaltet ist
        if (FinalGateController.Instance != null && FinalGateController.Instance.GetComponent<Collider>().enabled)
        {
            playerInRange = true;
            promptUI?.SetActive(true);
            promptText.text = "Press E to enter";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        promptUI?.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            promptUI?.SetActive(false);
            FinalGateController.Instance?.Interact();
        }
    }
}