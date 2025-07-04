// HubEffectController.cs
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class LevelOutcome
{
    public string levelID;
    public GameObject leftOutcomeObject;
    public GameObject rightOutcomeObject;
}

public class HubEffectController : MonoBehaviour
{
    public List<LevelOutcome> levelOutcomes = new List<LevelOutcome>();

    void OnEnable()
    {
        GameManager.Instance.OnFlashbackChoiceChanged += Apply;
        foreach (var o in levelOutcomes)
            Apply(o.levelID, GameManager.Instance.GetFlashbackChoice(o.levelID));
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnFlashbackChoiceChanged -= Apply;
    }

    private void Apply(string levelID, FlashbackChoice choice)
    {
        var o = levelOutcomes.Find(x => x.levelID == levelID);
        if (o == null) return;
        if (o.leftOutcomeObject) o.leftOutcomeObject.SetActive(choice == FlashbackChoice.ChoseLeft);
        if (o.rightOutcomeObject) o.rightOutcomeObject.SetActive(choice == FlashbackChoice.ChoseRight);
    }
}
