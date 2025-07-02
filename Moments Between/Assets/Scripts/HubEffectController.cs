// HubEffectController.cs
// Führt Effekte in der Hub-Szene basierend auf mehreren Entscheidungen aus.
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class LevelOutcome
{
    [Tooltip("Level-ID passend zu FlashbackDecisionTrigger.levelID")] public string levelID;
    public GameObject leftOutcomeObject;
    public GameObject rightOutcomeObject;
}

public class HubEffectController : MonoBehaviour
{
    [Header("Level Outcomes")]
    [Tooltip("Konfiguration der Auswirkungen pro Flashback-Level")]
    public List<LevelOutcome> levelOutcomes = new List<LevelOutcome>();

    void OnEnable()
    {
        GameManager.Instance.OnFlashbackChoiceChanged += ApplyChoice;
        foreach (var outcome in levelOutcomes)
        {
            var choice = GameManager.Instance.GetFlashbackChoice(outcome.levelID);
            ApplyChoice(outcome.levelID, choice);
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnFlashbackChoiceChanged -= ApplyChoice;
    }

    private void ApplyChoice(string levelID, FlashbackChoice choice)
    {
        var outcome = levelOutcomes.Find(o => o.levelID == levelID);
        if (outcome == null) return;
        if (outcome.leftOutcomeObject) outcome.leftOutcomeObject.SetActive(false);
        if (outcome.rightOutcomeObject) outcome.rightOutcomeObject.SetActive(false);

        switch (choice)
        {
            case FlashbackChoice.ChoseLeft:
                if (outcome.leftOutcomeObject) outcome.leftOutcomeObject.SetActive(true);
                break;
            case FlashbackChoice.ChoseRight:
                if (outcome.rightOutcomeObject) outcome.rightOutcomeObject.SetActive(true);
                break;
        }
    }
}
