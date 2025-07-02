// FlashbackDecisionTrigger.cs
// Modifiziert den DecisionTrigger, um GameManager mit levelID zu informieren.
using UnityEngine;
using UnityEngine.Events;

public class FlashbackDecisionTrigger : MonoBehaviour
{
    [Header("References")]
    public DecisionTrigger decisionTrigger;    // Dein bestehender DecisionTrigger

    [Header("Level Info")]
    [Tooltip("Eindeutige ID des Flashback-Levels (z.B. 'Level1', 'Level2', ...)")]
    public string levelID;

    [Header("Scenes")]
    public string flashbackSceneName;
    public string hubSceneName;

    void Start()
    {
        decisionTrigger.onChooseLeft.AddListener(() => OnDecisionMade(FlashbackChoice.ChoseLeft));
        decisionTrigger.onChooseRight.AddListener(() => OnDecisionMade(FlashbackChoice.ChoseRight));
    }

    private void OnDecisionMade(FlashbackChoice choice)
    {
        GameManager.Instance.SetFlashbackChoice(levelID, choice);
        SceneTransitionManager.Instance.ReturnToHub(hubSceneName);
    }

    /// <summary>
    /// Ruft den Flashback-Level auf.
    /// </summary>
    public void StartFlashback()
    {
        SceneTransitionManager.Instance.LoadFlashback(flashbackSceneName);
    }
}