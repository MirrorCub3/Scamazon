using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField]
    private FadeScreen fadeScreen;

    bool fading;

    private void Awake()
    {
        fading = false;
    }

    public void GoToScene(string sceneName)
    {
        if (fading) return;
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        fading = true;
        fadeScreen.FadeOut();
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(sceneName);
        
        sceneLoading.allowSceneActivation = false;
        yield return new WaitForSeconds(fadeScreen.GetFadeDuration());
        sceneLoading.allowSceneActivation = true;
        
        while (!sceneLoading.isDone)
        {
            yield return null;
        }
    }
}
