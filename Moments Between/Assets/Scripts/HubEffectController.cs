// HubEffectController.cs
// Führt Effekte in der Hub-Szene basierend auf der Entscheidung aus.
using UnityEngine;

public class HubEffectController : MonoBehaviour
{
    [Header("Affected Objects")]
    public GameObject leftOutcomeObject;
    public GameObject rightOutcomeObject;

    void OnEnable()
    {
        GameManager.Instance.OnFlashbackChoiceChanged += ApplyChoice;
        // Bei Szene-Load ggf. vorhandene Wahl feststellen
        ApplyChoice(GameManager.Instance.flashbackChoice);
    }

    void OnDisable()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnFlashbackChoiceChanged -= ApplyChoice;
    }

    private void ApplyChoice(FlashbackChoice choice)
    {
        // Beide Outcomes zunächst deaktivieren
        leftOutcomeObject?.SetActive(false);
        rightOutcomeObject?.SetActive(false);

        switch (choice)
        {
            case FlashbackChoice.ChoseLeft:
                leftOutcomeObject?.SetActive(true);
                break;
            case FlashbackChoice.ChoseRight:
                rightOutcomeObject?.SetActive(true);
                break;
            default:
                // Keine Entscheidung getroffen
                break;
        }
    }
}