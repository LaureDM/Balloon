﻿using UnityEngine;
using System.Collections;
using AssemblyCSharp.Code.Enums;
using AssemblyCSharp.Code.Trees;
using AssemblyCSharp.Code.Controllers;

public class PineTreeScript : MonoBehaviour, ITree
{
    private const float PARTICLE_DURATION = 4.0f;

    [SerializeField]
    private GameObject[] growStages;

    [SerializeField]
    private GameObject[] animals;

    [SerializeField]
    private float timeTillNextStage;

    [SerializeField]
    private float timeTillNextAnimal;

    [SerializeField]
    private float timeTillNextFruit;

    [SerializeField]
    private GameObject[] fruits;

    [SerializeField]
    private FruitSpawnPosition[] fruitSpawnPositions;

    [SerializeField]
    private GameObject seed;

    [SerializeField]
    private GameObject leavesParticlesPrefab;

    private TreeCollectionScript treeCollection;

    private AnimalSpawnerScript animalSpawner;

    private Transform currentStagePrefab;
    private GrowState currentStage = GrowState.SEED;
    private float startTime;
    private float rabbitSpawnTime;
    private float fruitSpawnTime;

    private Rigidbody rigidBody;

    //todo temporary
    private bool isRabbitSpawned;

    private Vector3 terrainUp;

    private Vector3 treeScale;

    void Start()
    {
        currentStagePrefab = seed.transform;
        rigidBody = GetComponent<Rigidbody>();
        animalSpawner = FindObjectOfType<AnimalSpawnerScript>();
        treeCollection = FindObjectOfType<TreeCollectionScript>();
    }

    void Update()
    {
        //grow tree
        if (currentStage != GrowState.SEED && currentStage != GrowState.ADULT && IsReadyToGrow())
        {
            GrowToNextStage();
        }
        //when adult, spawn animals
        else if (currentStage == GrowState.ADULT && ShouldSpawnRabbit())
        {
            isRabbitSpawned = true;
            animalSpawner.SpawnAnimal(Animal.RABBIT);
            rabbitSpawnTime = Time.time;
        }
        //when adult, drop fruits
        else if (currentStage == GrowState.ADULT && ShouldSpawnFruit())
        {
            TryToSpawnFruit();
        }

    }

    private bool IsReadyToGrow()
    {
        return (Time.time - startTime) > timeTillNextStage;
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
                newStagePrefab = growStages[0];
                break;

            case GrowState.SEEDLING:

                currentStage = GrowState.SAPLING;
                newStagePrefab = growStages[1];
                break;

            case GrowState.SAPLING:

                currentStage = GrowState.ADULT;
                newStagePrefab = growStages[2];
                break;
        }

        startTime = Time.time;
        StartCoroutine(ChangeTree(newStagePrefab));
    }

    private IEnumerator ChangeTree(GameObject newStagePrefab)
    {
        float normalizedScale = treeScale.normalized.magnitude;
        float scaleSpeed = 6f;

        for (float s = normalizedScale; s >= 0; s-=Time.deltaTime * scaleSpeed)
        {
            currentStagePrefab.transform.localScale = new Vector3(s, s, s);
            yield return new WaitForEndOfFrame();
        }

        currentStagePrefab.transform.localScale = Vector3.zero;

        Destroy(currentStagePrefab.gameObject);

        GameObject treeStage = Instantiate(newStagePrefab, transform.position, transform.rotation) as GameObject;
        
        currentStagePrefab = treeStage.transform;
        currentStagePrefab.parent = gameObject.transform;
        currentStagePrefab.transform.up = terrainUp;

        for (float s = 0f; s < normalizedScale; s += Time.deltaTime * scaleSpeed)
        {
            currentStagePrefab.transform.localScale = new Vector3(s, s, s);
            yield return new WaitForEndOfFrame();
        }

        currentStagePrefab.transform.localScale = treeScale;

        float particleHeight = (transform.position.y + currentStagePrefab.GetComponentInChildren<Collider>().bounds.size.y)/2;
        Vector3 particlesPosition = new Vector3(transform.position.x, particleHeight, transform.position.z);

        GameObject particlesParent = Instantiate(leavesParticlesPrefab, particlesPosition, transform.rotation) as GameObject;
        ParticleSystem particles = particlesParent.GetComponentInChildren<ParticleSystem>();
        ParticleSystem particlesSubEmitter = particles.GetComponentInChildren<ParticleSystem>();
        Destroy(particlesParent, PARTICLE_DURATION);
    }

    bool ShouldSpawnRabbit()
    {
        return (Time.time - rabbitSpawnTime) > timeTillNextAnimal && !isRabbitSpawned;
    }

    bool ShouldSpawnFruit()
    {
        return (Time.time - fruitSpawnTime) > timeTillNextFruit;
    }

    void TryToSpawnFruit()
    {
        foreach (FruitSpawnPosition fruitSpawnPosition in fruitSpawnPositions)
		{
			if (!fruitSpawnPosition.IsFruitSpawned())
			{
                fruitSpawnPosition.SpawnFruit(FruitType.APPLE);
                fruitSpawnTime = Time.time;
			}
		}
    }

    public void OnCollisionEnter(Collision collider)
	{
		//seed hits the ground
		if (collider.gameObject.GetComponent<TerrainScript>() != null)
		{
			rigidBody.isKinematic = true;

            gameObject.transform.parent = treeCollection.gameObject.transform;

            terrainUp = collider.gameObject.transform.up;

			float randomY = Random.Range(0, 360);
			float randomScale = Random.Range(0.6f, 1.0f);

			treeScale = new Vector3(randomScale, randomScale, randomScale);

            Vector3 treeRotation = new Vector3(0, randomY, 0);
            gameObject.transform.Rotate(treeRotation);

            GrowToNextStage();
		}
	}
}
