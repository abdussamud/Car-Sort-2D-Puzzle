using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableEvent : MonoBehaviour
{
    public UnityEvent eventCall;

    private void OnEnable()
    {
        eventCall.Invoke();
    }

    public void UpdateCoinsText(TextMeshProUGUI text)
    {
        text.text = DataManager.dm.gameData.coins.ToString();
    }
}
