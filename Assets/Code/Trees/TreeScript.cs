using UnityEngine;
using System.Collections;
using AssemblyCSharp.Code.Enums;
using AssemblyCSharp.Code.Controllers;
using System.Collections.Generic;
using System.Linq;

public class TreeScript : MonoBehaviour
{
    #region Constants

    private const float PARTICLE_DURATION = 3.0f;

    #endregion

    #region Editor Fields

    [SerializeField]
    private GrowstageDictionary growStages;

    [SerializeField]
    private GrowstageDurationDictionary growStageDurations;

    [SerializeField]
    private AnimalPercentageDictionary possibleAnimals;

    [SerializeField]
    private float timeTillNextAnimal;

    [SerializeField]
    private float timeTillNextFruit;

    [SerializeField]
    private FruitSpawnPosition fruitSpawnPosition;

    [SerializeField]
    private GameObject leavesParticlesPrefab;

    [SerializeField]
    private TreeType type;

    #endregion

    #region Fields

    private TreeCollectionScript treeCollection;

    private AnimalCollectionScript animalSpawner;

    private Transform currentStageTransform;
    private GrowState currentStage;
    private float startTime;
    private float animalSpawnTime;
    private float fruitSpawnTime;

    private Rigidbody rigidBody;

    private Vector3 terrainUp;

    private Vector3 treeScale;
    private float currentDuration;

    #endregion

    #region Initialization

    void Awake()
    {
        currentStage = GrowState.SEED;
        growStageDurations.TryGetValue(currentStage, out currentDuration);
        GameObject seedPrefab = null;
        growStages.TryGetValue(currentStage, out seedPrefab);

        GameObject seed = Instantiate(seedPrefab, transform.position, transform.rotation, gameObject.transform) as GameObject;
        currentStageTransform = seed.transform;

    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animalSpawner = FindObjectOfType<AnimalCollectionScript>();
        treeCollection = FindObjectOfType<TreeCollectionScript>();
    }

    #endregion

    #region Unity Methods

    void Update()
    {
        //grow tree
        if (currentStage != GrowState.SEED && currentStage != GrowState.ADULT && IsReadyToGrow())
        {
            GrowToNextStage();
        }
        //when adult, spawn animals
        else if (currentStage == GrowState.ADULT && ShouldSpawnAnimal())
        {
            AnimalType animal = CalculateAnimalToSpawn();
            Debug.Log(animal);
            animalSpawner.SpawnAnimal(animal);
            animalSpawnTime = Time.time;
        }
        //when adult, drop fruits
        else if (currentStage == GrowState.ADULT && ShouldSpawnFruit())
        {
            TryToSpawnFruit();
        }


        //TODO write test code to check gone/restored events

    }

    public void OnCollisionEnter(Collision collider)
    {
        //seed hits the ground
        if (collider.gameObject.GetComponent<TerrainScript>() != null)
        {
            rigidBody.isKinematic = true;

            gameObject.transform.parent = treeCollection.gameObject.transform;
            treeCollection.IncreaseTreeCount(type);

            terrainUp = collider.gameObject.transform.up;

            float randomY = Random.Range(0, 360);
            float randomScale = Random.Range(0.6f, 1.0f);

            treeScale = new Vector3(randomScale, randomScale, randomScale);

            Vector3 treeRotation = new Vector3(0, randomY, 0);
            gameObject.transform.Rotate(treeRotation);
            gameObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

            GrowToNextStage();
        }
    }

    #endregion

    #region Helper Methods

    private bool IsReadyToGrow()
    {
        return (Time.time - startTime) > currentDuration;
    }

    public bool IsAdult()
    {
        return currentStage == GrowState.ADULT;
    }

    private void GrowToNextStage()
    {
        GameObject newStagePrefab = null;

        switch (currentStage)
        {
            case GrowState.SEED:

                currentStage = GrowState.SEEDLING;
                break;

            case GrowState.SEEDLING:

                currentStage = GrowState.SAPLING;
                break;

            case GrowState.SAPLING:

                currentStage = GrowState.ADULT;
                treeCollection.IncreaseAdultTreeCount(type);
                break;
        }

        growStages.TryGetValue(currentStage, out newStagePrefab);
        growStageDurations.TryGetValue(currentStage, out currentDuration);
        startTime = Time.time;
        StartCoroutine(ChangeTree(newStagePrefab));
    }

    private bool ShouldSpawnAnimal()
    {
        return (Time.time - animalSpawnTime) > timeTillNextAnimal;
    }

    private bool ShouldSpawnFruit()
    {
        return (Time.time - fruitSpawnTime) > timeTillNextFruit;
    }

    void TryToSpawnFruit()
    {
        if (!fruitSpawnPosition.IsFruitSpawned())
        {
            fruitSpawnPosition.SpawnFruit();
            fruitSpawnTime = Time.time;
        }
    }

    private AnimalType CalculateAnimalToSpawn()
    {
        int randomNumber = Random.Range(0, 100);
        int percentage = 0;

        foreach (KeyValuePair<AnimalType, int> entry in possibleAnimals)
        {
            percentage += entry.Value;

            if (randomNumber <= percentage)
            {
                return entry.Key;
            }
        }

        //if for some reason something goes wrong, return the first animal possible
        return possibleAnimals.First().Key;
    }

    #endregion

    #region Coroutines

    private IEnumerator ChangeTree(GameObject newStagePrefab)
    {
        float normalizedScale = treeScale.normalized.magnitude;
        float scaleSpeed = 6f;
        
        for (float s = normalizedScale; s >= 0; s-=Time.deltaTime * scaleSpeed)
        {
            currentStageTransform.transform.localScale = new Vector3(s, s, s);
            yield return new WaitForEndOfFrame();
        }

        currentStageTransform.transform.localScale = Vector3.zero;

        Destroy(currentStageTransform.gameObject);

        GameObject treeStage = Instantiate(newStagePrefab, transform.position, transform.rotation) as GameObject;
        
        currentStageTransform = treeStage.transform;
        currentStageTransform.parent = gameObject.transform;
        currentStageTransform.transform.up = terrainUp;

        for (float s = 0f; s < normalizedScale; s += Time.deltaTime * scaleSpeed)
        {
            currentStageTransform.transform.localScale = new Vector3(s, s, s);
            yield return new WaitForEndOfFrame();
        }

        currentStageTransform.transform.localScale = treeScale;

        float particleHeight = (transform.position.y + currentStageTransform.GetComponentInChildren<Collider>().bounds.size.y)/2;
        Vector3 particlesPosition = new Vector3(transform.position.x, particleHeight, transform.position.z);

        GameObject particles = Instantiate(leavesParticlesPrefab, particlesPosition, transform.rotation) as GameObject;
        Destroy(particles, PARTICLE_DURATION);
    }

    #endregion
}
