using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Slider loadingSlider;


    private void Start()
    {
        sceneName = GameManager.Instance.nextScene;
        if (sceneName == string.Empty) { sceneName = "Main Menu"; }
        _ = loadingSlider.DOValue(1, 4);
        _ = StartCoroutine(StartLoading());
    }

    private IEnumerator StartLoading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(5f);
        operation.allowSceneActivation = true;
    }
}

/*
//private float _target;
private async void Loading()
{
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
    asyncLoad.allowSceneActivation = false;
    await Task.Delay(5000);
    asyncLoad.allowSceneActivation = true;
    //UnityEngine.Debug.Log("Function running time: " + stopwatch.ElapsedMilliseconds + " ms");
}

private async void LoadScene()
{
    AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
    scene.allowSceneActivation = false;

    do
    {
        await Task.Delay(100);
        _target = scene.progress + 0.1f;
    }
    while (scene.progress < 0.9f);

    await Task.Delay(2500);
    scene.allowSceneActivation = true;
}

private void MyUpdate()
{
    loadingSlider.value = Mathf.MoveTowards(loadingSlider.value, _target, 0.4f * Time.deltaTime);
}
//Loading();
//LoadScene();
//using System.Diagnostics;
//Stopwatch stopwatch = new();
//stopwatch.Start();
//UnityEngine.Debug.Log("Function running time: " + stopwatch.ElapsedMilliseconds + " ms");
*/
