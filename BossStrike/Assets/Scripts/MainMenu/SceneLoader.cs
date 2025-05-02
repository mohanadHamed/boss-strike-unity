using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private Slider _loadingBar;

    void Start()
    {
        _loadingBar.gameObject.SetActive(false);
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        _loadingBar.gameObject.SetActive(true);
        _loadingBar.value = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Progress is [0, 0.9] — Unity waits at 0.9 until allowSceneActivation = true
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            _loadingBar.value = progress;

            if (operation.progress >= 0.9f)
            {
                // Optional delay before activation (or wait for user input)
                yield return new WaitForSeconds(1f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    // Optional: use this if loading is triggered from a button
    public void LoadTargetScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
}
