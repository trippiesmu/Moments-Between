using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Lines")]
    [TextArea] public string[] lines;

    [Header("Scene Configuration")]
    [Tooltip("Exakter Name der Flashback-Szene, wie in den Build Settings")]
    public string flashbackSceneName;

    /// <summary>
    /// Wird von InteractionSystem aufgerufen, sobald der Spieler E drückt.
    /// </summary>
    public void TriggerDialogue()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(lines, flashbackSceneName);
        }
        else
        {
            Debug.LogError($"[{name}] Kein DialogueManager in der Szene!");
        }
    }
}