using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FlashbackDecisionTrigger : MonoBehaviour
{
    [Header("References")]
    public DecisionTrigger decisionTrigger;  // Verweis auf das bereits definierte DecisionTrigger

    [Header("Scenes")]
    public string flashbackSceneName;
    public string hubSceneName;

    void Start()
    {
        // Hook ins Event des DecisionTrigger
        decisionTrigger.onChooseLeft.AddListener(() => OnDecisionMade(FlashbackChoice.ChoseLeft));
        decisionTrigger.onChooseRight.AddListener(() => OnDecisionMade(FlashbackChoice.ChoseRight));
    }

    private void OnDecisionMade(FlashbackChoice choice)
    {
        // Speichere Entscheidung global
        GameManager.Instance.SetFlashbackChoice(choice);
        // Szene wechseln zurück zur Hub
        SceneTransitionManager.Instance.ReturnToHub(hubSceneName);
    }

    public void StartFlashback()
    {
        // Entscheidungsszene starten
        SceneTransitionManager.Instance.LoadFlashback(flashbackSceneName);
    }
}