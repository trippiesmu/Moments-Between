// HubTutorial.cs
using UnityEngine;
using TMPro;

public class HubTutorial : MonoBehaviour
{
    // Persistenter Flag, bleibt während des gesamten Laufs erhalten
    private static bool tutorialShown = false;

    [Header("UI References")]
    [Tooltip("Panel mit dem Tutorial-Text")]
    public GameObject tutorialPanel;
    [Tooltip("TextMeshProUGUI im Tutorial-Panel")]
    public TextMeshProUGUI tutorialText;

    [Header("Instructions")]
    [TextArea]
    [Tooltip("Steuerungshinweise, z.B. WASD...")]
    public string instructions = 
        "WASD – bewegen\n" +
        "E – mit Objekten interagieren\n" +
        "Pfeiltasten – Entscheidungen treffen\n\n" +
        "Drücke eine beliebige Taste, um fortzufahren.";

    bool isShowing = false;

    void Start()
    {
        // Wenn wir das Tutorial bereits einmal komplett angezeigt hatten,
        // zerstören wir direkt das Panel und deaktivieren dieses Script.
        if (tutorialShown)
        {
            if (tutorialPanel != null)
                Destroy(tutorialPanel);
            Destroy(this);
            return;
        }

        // Ansonsten Tutorial anzeigen
        if (tutorialPanel != null && tutorialText != null)
        {
            tutorialText.text = instructions;
            tutorialPanel.SetActive(true);
            isShowing = true;
        }
        else
        {
            Debug.LogError("HubTutorial: UI-Referenzen fehlen!");
            Destroy(this);
        }
    }

    void Update()
    {
        if (!isShowing) return;

        // Sobald irgendeine Taste oder Mausklick:
        if (Input.anyKeyDown)
        {
            // Flag setzen, damit es nie wieder angezeigt wird
            tutorialShown = true;

            // Panel vollständig entfernen
            if (tutorialPanel != null)
                Destroy(tutorialPanel);

            isShowing = false;
            Destroy(this);
        }
    }
}