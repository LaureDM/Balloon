using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Fruit : MonoBehaviour
{
    public List<BaseAnimal> approachingAnimals;

    // public bool IsLocked { get; set; }

    void Awake()
    {
        approachingAnimals = new List<BaseAnimal>();
    }

    public void target(BaseAnimal animal)
    {
        Debug.Log("Rabbit added to incoming");
        approachingAnimals.Add(animal);
    }

    public void unTarget(BaseAnimal animal)
    {
        // TODO animal doesn't always change target?
        // TODO make animale rest first before picking new target?

        Debug.Log("Removing rabbit from incoming");

        approachingAnimals.Remove(animal);
    }

    void OnDestroy()
    {
        Debug.Log("fruit destroyed");

        Debug.Log("Notifying: " + approachingAnimals.Count + " animals");
        foreach (BaseAnimal animal in approachingAnimals)
        {
            Debug.Log("Notified animal");
            animal.TargetLost();
        }
    }
}
