using UnityEngine;
using System.Collections.Generic;

public interface IObserver
{
    public void OnNotify<T>(T data);
}

public class  ObserverSubject: MonoBehaviour
{
    readonly Dictionary<string, IObserver> observers = new();


    public void AddObserver(string key, IObserver observer)
    {
        if (observer == null) return;
        observers.Add(key, observer);
    }

    public void RemoveObserver(string key)
    {
        observers.Remove(key);
    }

    protected void NotifyObservers<T>(T data)
    {
        foreach(KeyValuePair<string, IObserver> observer in observers)
        {
            observer.Value.OnNotify(data);
        }
    }
}

