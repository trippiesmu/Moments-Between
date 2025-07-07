using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("Unique ID (optional)")]
    [Tooltip("Eindeutige ID dieses Objekts. Leer = GameObject.name")]
    public string objectID;

    [Header("Dialogue Lines")]
    [TextArea] public string[] lines;

    [Header("Scene Configuration")]
    [Tooltip("Exakter Name der Flashback-Szene, wie in Build Settings")]
    public string flashbackSceneName;

    [Header("Effect Prefab")]
    [Tooltip("Prefab mit Partikeln/Outline/etc.")]
    public GameObject interactionEffectPrefab;

    [Header("Effect Settings")]
    [Tooltip("Versatz relativ zur Objekt-Position")]
    public Vector3 effectOffset = default;

    // Laufzeit
    GameObject effectInstance;
    bool hasInteracted;

    void Awake()
    {
        // Bestimme ID
        if (string.IsNullOrEmpty(objectID))
            objectID = gameObject.name;

        // Wurde schon interagiert?
        hasInteracted = InteractionStore.Has(objectID);

        // Collider gleich deaktivieren, wenn schon durchlaufen
        if (hasInteracted)
            GetComponent<Collider>().enabled = false;
    }

    void Start()
    {
        // Nur einmalig den Effekt instantiieren, falls noch nicht interagiert
        if (!hasInteracted && interactionEffectPrefab != null)
        {
            Vector3 spawnPos = transform.position + effectOffset;
            effectInstance = Instantiate(interactionEffectPrefab, spawnPos, Quaternion.identity, transform);
            effectInstance.SetActive(true);
        }
    }

    /// <summary>
    /// Wird vom InteractionSystem per E-Taste aufgerufen.
    /// </summary>
    public void TriggerDialogue()
    {
        // Speichere die Interaktion
        if (!hasInteracted)
        {
            InteractionStore.Add(objectID);
            hasInteracted = true;

            // Collider und Effekt aus
            GetComponent<Collider>().enabled = false;
            if (effectInstance != null)
            {
                Destroy(effectInstance);
                effectInstance = null;
            }
        }

        // Starte den Dialog
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.StartDialogue(lines, flashbackSceneName);
        else
            Debug.LogError($"[{name}] DialogueManager nicht gefunden!");
    }
}
