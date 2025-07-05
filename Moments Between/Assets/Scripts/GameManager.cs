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
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetChoice(string levelID, FlashbackChoice choice)
    {
        choices[levelID] = choice;
        OnChoiceChanged?.Invoke(levelID, choice);
    }

    public FlashbackChoice GetChoice(string levelID)
        => choices.TryGetValue(levelID, out var c) ? c : FlashbackChoice.None;
}