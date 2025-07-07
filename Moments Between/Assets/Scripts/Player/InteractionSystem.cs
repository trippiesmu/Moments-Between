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

    Camera playerCamera;
    DialogueTrigger currentFocus;
    bool isShowingPrompt;

    void Start()
    {
        playerCamera = Camera.main;
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        // wenn Dialog läuft → keine Prompts
        if (DialogueManager.Instance != null && DialogueManager.Instance.DialogueActive)
        {
            ClearFocus();
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out var hit, interactionDistance, interactableLayer)
            && hit.collider.TryGetComponent<DialogueTrigger>(out var dt))
        {
            currentFocus = dt;

            if (!isShowingPrompt && promptUI != null)
            {
                promptUI.SetActive(true);
                isShowingPrompt = true;
            }
            promptText.text = "Press E to interact";

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentFocus != null)
                {
                    promptUI.SetActive(false);
                    isShowingPrompt = false;
                    currentFocus.TriggerDialogue();
                    currentFocus = null;
                }
            }
        }
        else
        {
            ClearFocus();
        }
    }

    void ClearFocus()
    {
        currentFocus = null;
        if (isShowingPrompt && promptUI != null)
        {
            promptUI.SetActive(false);
            isShowingPrompt = false;
        }
    }
}