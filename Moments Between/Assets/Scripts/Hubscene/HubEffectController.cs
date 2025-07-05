// HubEffectController.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Persistenter Controller, der nach jedem Laden der Hub-Szene
/// alle OutcomeObject-Marker (auch inaktive) sammelt und entsprechend
/// der gespeicherten Choice aktiviert/deaktiviert.
/// </summary>
public class HubEffectController : MonoBehaviour
{
    public static HubEffectController Instance { get; private set; }

    [Tooltip("Exakter Name deiner Hub-Szene, wie in Build Settings")]
    public string hubSceneName;

    // levelID → (leftObj, rightObj)
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
        // nur in der Hub-Szene reagieren
        if (scene.name != hubSceneName) return;

        BuildOutcomeMap();    // sammelt auch inaktive Marker
        ApplyAllEffects();    // aktiviert/deaktiviert korrekt
    }

    private void BuildOutcomeMap()
    {
        outcomes.Clear();

        // FindObjectsOfTypeAll findet auch deaktivierte GameObjects
        foreach (var marker in Resources.FindObjectsOfTypeAll<OutcomeObject>())
        {
            // nur Marker in der aktuell geladenen Hub-Szene berücksichtigen
            if (marker.gameObject.scene.name != hubSceneName) continue;

            if (!outcomes.ContainsKey(marker.levelID))
                outcomes[marker.levelID] = (null, null);

            var tuple = outcomes[marker.levelID];
            if (marker.isLeftOutcome)
                tuple.left = marker.gameObject;
            else
                tuple.right = marker.gameObject;
            outcomes[marker.levelID] = tuple;
        }
    }

    private void ApplyAllEffects()
    {
        foreach (var kvp in outcomes)
        {
            string levelID = kvp.Key;
            FlashbackChoice choice = GameManager.Instance.GetChoice(levelID);

            var (leftObj, rightObj) = kvp.Value;
            if (leftObj  != null) leftObj .SetActive(choice == FlashbackChoice.None);
            if (rightObj != null) rightObj.SetActive(choice == FlashbackChoice.ChoseRight);
        }
    }
}
