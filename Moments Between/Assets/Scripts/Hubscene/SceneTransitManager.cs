// SceneTransitionManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadHub(string sceneName)      => StartCoroutine(LoadSceneAsync(sceneName));
    public void LoadFlashback(string sceneName) => StartCoroutine(LoadSceneAsync(sceneName));

    private IEnumerator LoadSceneAsync(string name)
    {
        var op = SceneManager.LoadSceneAsync(name);
        while (!op.isDone) yield return null;
    }
}