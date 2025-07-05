using UnityEngine;
using TMPro;

public class Level2QuestLog : MonoBehaviour
{
    [Header("UI References")]
    public GameObject questUI;           // Panel-GameObject
    public TextMeshProUGUI questText;    // Textfeld im Panel

    [Header("Quest Steps")]
    [TextArea] public string[] steps;    // Deine Anweisungen

    private int currentStep = 0;

    void Start()
    {
        if (steps == null || steps.Length == 0)
        {
            questUI?.SetActive(false);
            return;
        }

        currentStep = 0;
        UpdateQuestUI();
        questUI?.SetActive(true);
    }

    /// <summary>
    /// Ruft den nächsten Quest-Schritt auf.
    /// </summary>
    public void CompleteStep()
    {
        currentStep++;
        if (currentStep < steps.Length)
        {
            UpdateQuestUI();
            questUI?.SetActive(true);
        }
        else
        {
            // alle Schritte geschafft → Log ausblenden
            questUI?.SetActive(false);
        }
    }

    private void UpdateQuestUI()
    {
        if (questText != null)
            questText.text = steps[currentStep];
    }
}