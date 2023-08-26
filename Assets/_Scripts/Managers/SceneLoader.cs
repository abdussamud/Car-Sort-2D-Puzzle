using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Slider loadingSlider;
    private float loadingTime = 4;

    private void Start()
    {
        loadingTime = GameManager.Instance.startLoadingDone ? 4 : 8;
        if (!GameManager.Instance.startLoadingDone)
        {
            loadingTime = 8;
            GameManager.Instance.startLoadingDone = true;
        }
        sceneName = GameManager.Instance.nextScene;
        sceneName = sceneName == string.Empty ? "Main Menu" : sceneName;
        _ = loadingSlider.DOValue(1, loadingTime);
        _ = StartCoroutine(StartLoading());
    }

    private IEnumerator StartLoading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(loadingTime + 1);
        operation.allowSceneActivation = true;
    }
}
