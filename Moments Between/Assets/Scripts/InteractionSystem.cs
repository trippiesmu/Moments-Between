// InteractionSystem.cs
using UnityEngine;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;

    [Header("UI References")]
    [Tooltip("Panel-GameObject, das den ‘Press E to interact’-Text enthält")]
    public GameObject promptUI;
    [Tooltip("TextMeshProUGUI-Komponente im Panel")]
    public TextMeshProUGUI promptText;

    private Camera playerCamera;
    private bool isShowing = false;

    void Start()
    {
        playerCamera = Camera.main;

        // Panel unbedingt im Inspector zuweisen!
        if (promptUI == null || promptText == null)
        {
            Debug.LogError("InteractionSystem: promptUI und promptText müssen im Inspector gesetzt werden!");
        }
        else
        {
            promptUI.SetActive(false);
        }
    }

    void Update()
    {
        // Guard: wenn Referenzen fehlen, gar nichts tun
        if (promptUI == null || promptText == null || playerCamera == null)
            return;

        // (Rest wie gehabt)
        // Raycast prüfen...
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out var hit, interactionDistance, interactableLayer)
            && hit.collider.TryGetComponent<DialogueTrigger>(out var dt))
        {
            if (!isShowing)
            {
                promptUI.SetActive(true);
                isShowing = true;
            }
            promptText.text = "Press E to interact";

            if (Input.GetKeyDown(KeyCode.E))
            {
                promptUI.SetActive(false);
                isShowing = false;
                dt.TriggerDialogue();
            }
        }
        else if (isShowing)
        {
            promptUI.SetActive(false);
            isShowing = false;
        }
    }
}
