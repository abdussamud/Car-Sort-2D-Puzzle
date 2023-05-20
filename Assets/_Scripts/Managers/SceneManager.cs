using DG.Tweening;
using System.Collections;
//using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Image loadingInner;
    //Stopwatch stopwatch = new();


    private void Start()
    {
        sceneName = GameManager.Instance.nextScene;
        if (sceneName == string.Empty) { sceneName = "Main Menu"; }
        _ = loadingInner.DOFillAmount(1, 4);
        //stopwatch.Start();
        //_ = StartCoroutine(StartLoading());
        Loading();
    }

    private IEnumerator StartLoading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(5f);
        operation.allowSceneActivation = true;
        //UnityEngine.Debug.Log("Function running time: " + stopwatch.ElapsedMilliseconds + " ms");
    }

    private async void Loading()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        await Task.Delay(5000);
        asyncLoad.allowSceneActivation = true;
        //UnityEngine.Debug.Log("Function running time: " + stopwatch.ElapsedMilliseconds + " ms");
    }
}
