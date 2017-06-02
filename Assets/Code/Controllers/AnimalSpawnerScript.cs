using UnityEngine;
using System.Collections;
using AssemblyCSharp.Code.Enums;
using System.Collections.Generic;

public class AnimalSpawnerScript : MonoBehaviour {

    [SerializeField]
    private GameObject rabbitPrefab;

    //discovered animals with their count in the forest
    private Dictionary<Animal, int> animalDictionary;

	// Use this for initialization
	void Start () {
        animalDictionary = new Dictionary<Animal, int>();
	}

    public void SpawnAnimal(Animal animal)
    {
        GameObject prefab = null;

        switch(animal)
        {
            case Animal.RABBIT:
                prefab = rabbitPrefab;
                break;
        }

        GameObject animalObject = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
        animalObject.transform.parent = gameObject.transform;

        IncreaseAnimalCount(animal);
	}

    private void IncreaseAnimalCount(Animal animal)
    {
        if (animalDictionary.ContainsKey(animal))
        {
            animalDictionary[animal] = animalDictionary[animal] + 1;
        }
        else
        {
            animalDictionary.Add(animal, 1);
            //TODO this means it's a new animal
            //TODO send message to UI
        }
    }

    /*
    public void DecreaseAnimalCount(Animal animal)
    {
		if (animalDictionary.ContainsKey(animal) && animalDictionary.TryGetValue(animal, out int currentCount))
		{
            animalDictionary[animal] = currentCount - 1 < 0 ? 0 : currentCount - 1;
		}
    }
    */
}
