// InteractionSystem.cs
// Erlaubt dem Spieler, im Hub-Szene mit Objekten zu interagieren und eine Dialogsequenz zu starten.
using UnityEngine;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;
    public TextMeshProUGUI promptText;

    private DialogueTrigger currentDialogueTrigger;

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
        {
            var dt = hit.collider.GetComponent<DialogueTrigger>();
            if (dt != null)
            {
                promptText.gameObject.SetActive(true);
                promptText.text = "Press E to interact";
                currentDialogueTrigger = dt;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    promptText.gameObject.SetActive(false);
                    currentDialogueTrigger.TriggerDialogue();
                }
                return;
            }
        }
        promptText.gameObject.SetActive(false);
        currentDialogueTrigger = null;
    }
}