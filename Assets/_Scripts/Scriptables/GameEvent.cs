using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Object/Game Event", order = 2)]
public class GameEvent : ScriptableObject
{
    [SerializeField]
    private List<GameEventListener> listeners = new();

    public void TrigerEvent()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered();
        }
    }

    public void AddListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(GameEventListener listener)
    {
        _ = listeners.Remove(listener);
    }
}
