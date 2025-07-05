// SceneTransitionManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>Persistenter Manager zum Laden von Flashback- & Hub-Szenen.</summary>
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

    /// <summary>Flashback-Szene asynchron laden.</summary>
    public void LoadFlashback(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    /// <summary>Zur Hub-Szene zurückkehren.</summary>
    public void ReturnToHub(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string name)
    {
        var op = SceneManager.LoadSceneAsync(name);
        while (!op.isDone) yield return null;
    }
}