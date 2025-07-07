using UnityEngine;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;

    [Header("UI References")]
    public GameObject promptUI;
    public TextMeshProUGUI promptText;

    private Camera playerCamera;
    private bool isShowing;

    void Start()
    {
        playerCamera = Camera.main;
        if (promptUI != null) promptUI.SetActive(false);
    }

    void Update()
    {
        // Bei aktivem Dialog keine Prompts mehr
        if (DialogueManager.Instance != null && DialogueManager.Instance.DialogueActive)
            return;

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