// GameManager.cs
// Singleton, hält den persistenten Spielzustand (Entscheidungen für mehrere Flashback-Levels).
using UnityEngine;
using System;
using System.Collections.Generic;

public enum FlashbackChoice { None, ChoseLeft, ChoseRight }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Dictionary speichert Entscheidungen pro Level (levelID -> FlashbackChoice)
    private Dictionary<string, FlashbackChoice> flashbackChoices = new Dictionary<string, FlashbackChoice>();

    // Event, das ausgelöst wird, wenn eine Entscheidung für einen Level geändert wird
    public event Action<string, FlashbackChoice> OnFlashbackChoiceChanged;

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

    /// <summary>
    /// Setzt die Entscheidung für einen Level und feuert das Event.
    /// </summary>
    public void SetFlashbackChoice(string levelID, FlashbackChoice choice)
    {
        flashbackChoices[levelID] = choice;
        OnFlashbackChoiceChanged?.Invoke(levelID, choice);
    }

    /// <summary>
    /// Gibt die gespeicherte Entscheidung für einen Level zurück.
    /// </summary>
    public FlashbackChoice GetFlashbackChoice(string levelID)
    {
        if (flashbackChoices.TryGetValue(levelID, out var choice))
            return choice;
        return FlashbackChoice.None;
    }
}