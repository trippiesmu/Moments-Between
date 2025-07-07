// GameManager.cs
using UnityEngine;
using System;
using System.Collections.Generic;

public enum FlashbackChoice
{
    Unplayed,   // noch nicht gespielt
    ChoseLeft,  // links gewählt
    ChoseRight  // rechts gewählt
}

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

    public void SetChoice(string levelID, FlashbackChoice choice)
    {
        choices[levelID] = choice;
        OnChoiceChanged?.Invoke(levelID, choice);
    }

    public bool HasChoice(string levelID)
    {
        return choices.ContainsKey(levelID);
    }

    public FlashbackChoice GetChoice(string levelID)
    {
        return choices.TryGetValue(levelID, out var c) ? c : FlashbackChoice.Unplayed;
    }

    public int ChoiceCount => choices.Count;
}
