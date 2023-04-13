using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingHandler : MonoBehaviour
{

    [SerializeField] private Image loadingInner;

    private void OnEnable()
    {
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        loadingInner.DOFillAmount(1, 9);
        yield return new WaitForSecondsRealtime(9f);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        loadingInner.fillAmount = 0;
    }
}
