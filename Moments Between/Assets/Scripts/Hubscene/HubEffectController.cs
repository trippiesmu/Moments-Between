// HubEffectController.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HubEffectController : MonoBehaviour
{
    public static HubEffectController Instance { get; private set; }
    [Tooltip("Exakter Name deiner Hub-Szene, wie in Build Settings")]
    public string hubSceneName;

    // levelID → (leftMarker, rightMarker)
    private Dictionary<string, (GameObject left, GameObject right)> outcomes 
        = new Dictionary<string, (GameObject, GameObject)>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != hubSceneName) return;

        BuildOutcomeMap();
        ApplyAllEffects();
    }

    private void BuildOutcomeMap()
    {
        outcomes.Clear();
        foreach (var marker in Resources.FindObjectsOfTypeAll<OutcomeObject>())
        {
            if (marker.gameObject.scene.name != hubSceneName) continue;

            if (!outcomes.ContainsKey(marker.levelID))
                outcomes[marker.levelID] = (null, null);

            var tuple = outcomes[marker.levelID];
            if (marker.outcomeType == FlashbackChoice.ChoseLeft)
                tuple.left = marker.gameObject;
            else if (marker.outcomeType == FlashbackChoice.ChoseRight)
                tuple.right = marker.gameObject;
            outcomes[marker.levelID] = tuple;
        }
    }

    private void ApplyAllEffects()
    {
        foreach (var kvp in outcomes)
        {
            var levelID = kvp.Key;
            var choice  = GameManager.Instance.GetChoice(levelID);
            var (leftObj, rightObj) = kvp.Value;

            if (leftObj  != null) leftObj.SetActive(choice == FlashbackChoice.ChoseLeft);
            if (rightObj != null) rightObj.SetActive(choice == FlashbackChoice.ChoseRight);
        }
    }
}
