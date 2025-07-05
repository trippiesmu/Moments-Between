// HubInteractionDisabler.cs
using UnityEngine;

/// <summary>
/// Läuft beim Hub-Start und deaktiviert alle Hub-Trigger,
/// deren Flashback bereits gespielt wurde.
/// </summary>
public class HubInteractionDisabler : MonoBehaviour
{
    void Start()
    {
        var triggers = FindObjectsOfType<DialogueTrigger>();
        foreach (var trig in triggers)
        {
            if (GameManager.Instance.HasChoice(trig.flashbackSceneName))
            {
                // Collider & Script aus, so dass InteractionSystem nichts mehr findet
                trig.GetComponent<Collider>().enabled = false;
                trig.enabled = false;
            }
        }
    }
}