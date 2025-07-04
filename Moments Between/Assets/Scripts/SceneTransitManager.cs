// SceneTransitionManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Einmal in der Hub-Szene an ein GameObject hängen (z.B. "_Managers_").
/// Wird beim Laden des Spiels initialisiert und überlebt Szenewechsel.
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

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

    /// <summary>Flashback-Szene laden</summary>
    public void LoadFlashback(string sceneName)
        => StartCoroutine(LoadSceneAsync(sceneName));

    /// <summary>Optional: Zur Hub zurückkehren</summary>
    public void ReturnToHub(string sceneName)
        => StartCoroutine(LoadSceneAsync(sceneName));

    private IEnumerator LoadSceneAsync(string name)
    {
        var op = SceneManager.LoadSceneAsync(name);
        while (!op.isDone) yield return null;
    }
}