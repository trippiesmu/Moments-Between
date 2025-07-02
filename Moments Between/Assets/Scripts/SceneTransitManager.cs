// SceneTransitionManager.cs
// Verantwortlich fürs Laden und Zurückkehren zwischen Szenen.
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    /// <summary>
    /// Lädt eine Szene als Flashback-Level.
    /// </summary>
    public void LoadFlashback(string flashbackSceneName)
    {
        StartCoroutine(LoadSceneAsync(flashbackSceneName));
    }

    /// <summary>
    /// Kehrt in die Hub-Scene zurück.
    /// </summary>
    public void ReturnToHub(string hubSceneName)
    {
        StartCoroutine(LoadSceneAsync(hubSceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        var asyncOp = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOp.isDone)
            yield return null;
    }
}