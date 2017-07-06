using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Fruit : MonoBehaviour
{
    public List<UnityAction> approachingAnimals;

    // public bool IsLocked { get; set; }

    void Awake()
    {
        approachingAnimals = new List<UnityAction>();
    }

    public void target(UnityAction unityEvent)
    {
        approachingAnimals.Add(unityEvent);
    }

    public void unTarget(UnityAction unityEvent)
    {
        approachingAnimals.Remove(unityEvent);
    }

    void onDestroy()
    {
        foreach(UnityAction action in approachingAnimals)
        {
            action.Invoke();
        }
    }
}
