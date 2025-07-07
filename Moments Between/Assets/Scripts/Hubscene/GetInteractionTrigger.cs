// GateInteractionTrigger.cs
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider))]
public class GateInteractionTrigger : MonoBehaviour
{
    [Header("UI References")]
    public GameObject promptUI;           // Panel mit „Press E to enter“
    public TextMeshProUGUI promptText;

    bool playerInRange;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // OnTriggerEnter feuert nur, wenn col.enabled == true
        playerInRange = true;
        if (promptUI != null)
        {
            promptUI.SetActive(true);
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