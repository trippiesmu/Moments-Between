// GameManager.cs
using UnityEngine;
using System;
using System.Collections.Generic;

public enum FlashbackChoice { None, ChoseLeft, ChoseRight }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Dictionary<string, FlashbackChoice> flashbackChoices = new Dictionary<string, FlashbackChoice>();
    public event Action<string, FlashbackChoice> OnFlashbackChoiceChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetFlashbackChoice(string levelID, FlashbackChoice choice)
    {
        flashbackChoices[levelID] = choice;
        OnFlashbackChoiceChanged?.Invoke(levelID, choice);
    }

    public FlashbackChoice GetFlashbackChoice(string levelID)
        => flashbackChoices.TryGetValue(levelID, out var c) ? c : FlashbackChoice.None;
}