// GameManager.cs
using UnityEngine;
using System;
using System.Collections.Generic;

public enum FlashbackChoice { None, ChoseRight }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Dictionary<string, FlashbackChoice> choices = new Dictionary<string, FlashbackChoice>();
    public event Action<string, FlashbackChoice> OnChoiceChanged;

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

    /// <summary>Speichert die Entscheidung bzw. markiert Level als gespielt.</summary>
    public void SetChoice(string levelID, FlashbackChoice choice)
    {
        choices[levelID] = choice;
        OnChoiceChanged?.Invoke(levelID, choice);
    }

    /// <summary>Gibt true, wenn dieses Level schon einmal gespielt wurde.</summary>
    public bool HasChoice(string levelID)
    {
        return choices.ContainsKey(levelID);
    }

    /// <summary>Liefert gespeicherte Wahl (oder None, falls nie gesetzt).</summary>
    public FlashbackChoice GetChoice(string levelID)
    {
        return choices.TryGetValue(levelID, out var c) ? c : FlashbackChoice.None;
    }

    /// <summary>Anzahl aller gespeicherten Entscheidungen/gespielten Level.</summary>
    public int ChoiceCount => choices.Count;
}
