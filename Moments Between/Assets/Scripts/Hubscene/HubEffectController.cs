// HubEffectController.cs
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelOutcome
{
    public string levelID;
    public GameObject straightOutcome;
    public GameObject rightOutcome;
}

public class HubEffectController : MonoBehaviour
{
    public List<LevelOutcome> outcomes;

    void OnEnable()
    {
        GameManager.Instance.OnChoiceChanged += Apply;
        foreach (var o in outcomes)
            Apply(o.levelID, GameManager.Instance.GetChoice(o.levelID));
    }
    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnChoiceChanged -= Apply;
    }
    void Apply(string levelID, FlashbackChoice choice)
    {
        var o = outcomes.Find(x => x.levelID == levelID);
        if (o == null) return;
        o.straightOutcome?.SetActive(choice == FlashbackChoice.None);
        o.rightOutcome?.SetActive(choice == FlashbackChoice.ChoseRight);
    }
}