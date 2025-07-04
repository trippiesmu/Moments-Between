// InteractionSystem.cs
using UnityEngine;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;
    public TextMeshProUGUI promptText;
    public Camera playerCamera;

    private DialogueTrigger currentDT;

    void Start()
    {
        if (promptText != null) promptText.gameObject.SetActive(false);
        if (playerCamera == null) playerCamera = Camera.main;
    }

    void Update()
    {
        if (playerCamera == null) return;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out var hit, interactionDistance, interactableLayer)
            && hit.collider.TryGetComponent(out DialogueTrigger dt))
        {
            currentDT = dt;
            if (promptText != null && !promptText.gameObject.activeSelf)
            {
                promptText.text = "Press E to interact";
                promptText.gameObject.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                promptText.gameObject.SetActive(false);
                currentDT.TriggerDialogue();
            }
            return;
        }
        if (promptText != null && promptText.gameObject.activeSelf)
            promptText.gameObject.SetActive(false);
        currentDT = null;
    }
}
