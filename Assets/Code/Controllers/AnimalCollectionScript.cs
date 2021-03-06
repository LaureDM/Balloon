﻿using UnityEngine;
using System.Collections;
using AssemblyCSharp.Code.Enums;
using System.Collections.Generic;

public class AnimalCollectionScript : MonoBehaviour {

    #region Editor Fields

    [SerializeField]
    private AnimalPrefabDictionary animals;

    #endregion

    #region Fields

    /*
     *  discovered animals with their count in the forest
     */
    private Dictionary<AnimalType, int> animalDictionary;

    #endregion

    #region Initialization

    void Start ()
    {
        animalDictionary = new Dictionary<AnimalType, int>();
	}

    #endregion

    #region Helper Methods

    public void SpawnAnimal(AnimalType animal)
    {
        GameObject prefab = null;

        animals.TryGetValue(animal, out prefab);

        if (prefab != null)
        {
            GameObject animalObject = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            animalObject.transform.parent = gameObject.transform;
            IncreaseAnimalCount(animal);
        }

	}

    private void IncreaseAnimalCount(AnimalType animal)
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

    
    public void DecreaseAnimalCount(AnimalType animal)
    {
        int currentCount = 0;

		if (animalDictionary.ContainsKey(animal) && animalDictionary.TryGetValue(animal, out currentCount))
		{
            animalDictionary[animal] = currentCount - 1 < 0 ? 0 : currentCount - 1;
		}
    }

    #endregion

}
