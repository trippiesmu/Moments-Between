// FlashbackDecisionTrigger.cs
// Befindet sich im Flashback-Level an demselben GameObject wie DecisionTrigger.
// Speichert links/rechts-Wahl und kehrt nach Hub zurück.
using UnityEngine;

public class FlashbackDecisionTrigger : MonoBehaviour
{
    [Tooltip("Eindeutige ID dieses Flashback-Levels (z.B. 'Level1')")]
    public string levelID;

    [Tooltip("Name der Hub-Szene zum Zurückkehren.")]
    public string hubSceneName;

    private DecisionTrigger decisionTrigger;

    void Awake()
    {
        // Suche die DecisionTrigger-Komponente am gleichen GameObject
        decisionTrigger = GetComponent<DecisionTrigger>();
        if (decisionTrigger == null)
        {
            Debug.LogError("FlashbackDecisionTrigger benötigt eine DecisionTrigger-Komponente!");
            enabled = false;
            return;
        }
    }

    void Start()
    {
        // Jetzt Listener hinzufügen
        decisionTrigger.onChooseLeft.AddListener(() => Commit(FlashbackChoice.ChoseLeft));
        decisionTrigger.onChooseRight.AddListener(() => Commit(FlashbackChoice.ChoseRight));
    }

    private void Commit(FlashbackChoice choice)
    {
        // Entscheidung speichern
        GameManager.Instance.SetFlashbackChoice(levelID, choice);
        // Zurück zur Hub
        SceneTransitionManager.Instance.ReturnToHub(hubSceneName);
    }
}
