using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;
    public UnityEvent onEventTriggered;


    private void OnEnable()
    {
        gameEvent.AddListener(this);
    }

    public void OnEventTriggered()
    {
        onEventTriggered.Invoke();
    }

    private void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }
}
