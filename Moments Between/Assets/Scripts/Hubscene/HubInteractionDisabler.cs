// HubInteractionDisabler.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubInteractionDisabler : MonoBehaviour
{
    void Start()
    {
        // Alle DialogueTrigger (auch deaktivierte, falls bereits abgeschaltet) finden
        var triggers = Object.FindObjectsByType<DialogueTrigger>(
            FindObjectsInactive.Include, 
            FindObjectsSortMode.None
        );

        foreach (var trig in triggers)
        {
            // Wenn für diese Szene schon eine Choice existiert...
            if (GameManager.Instance.HasChoice(trig.flashbackSceneName))
            {
                // Collider & Script deaktivieren
                var col = trig.GetComponent<Collider>();
                if (col != null) col.enabled = false;
                trig.enabled = false;
            }
        }
    }
}
