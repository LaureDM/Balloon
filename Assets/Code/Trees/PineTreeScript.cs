using UnityEngine;
using System.Collections;
using AssemblyCSharp.Code.Enums;
using AssemblyCSharp.Code.Trees;
using AssemblyCSharp.Code.Controllers;

public class PineTreeScript : MonoBehaviour, ITree
{

    [SerializeField]
    private GameObject[] growStages;

    [SerializeField]
    private GameObject[] animals;

    [SerializeField]
    private float timeTillNextStage;

    [SerializeField]
    private float timeTillNextAnimal;

    [SerializeField]
    private GameObject[] fruitsSpawnPositions;

    private TreeCollectionScript treeCollection;

    private AnimalSpawnerScript animalSpawner;

    private Transform currentStagePrefab;
    private GrowState currentStage = GrowState.SEED;
    private float startTime;
    private float rabbitSpawnTime;

    private Rigidbody rigidBody;

    //todo temporary
    private bool isRabbitSpawned;

    private Vector3 terrainUp;

    private Vector3 treeScale;
    private Vector3 treeRotation;

    void Start()
    {
        currentStagePrefab = transform.GetChild(0);
        rigidBody = GetComponent<Rigidbody>();
        animalSpawner = FindObjectOfType<AnimalSpawnerScript>();
        treeCollection = FindObjectOfType<TreeCollectionScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO maybe do with switch if it takes longer for each stage
        if (currentStage != GrowState.SEED && currentStage != GrowState.ADULT && IsReadyToGrow())
        {
            GrowToNextStage();
        }
        else if (currentStage == GrowState.ADULT && ShouldSpawnRabbit())
        {
            isRabbitSpawned = true;
            //TODO drop fruits?
            animalSpawner.SpawnAnimal(Animal.RABBIT);
            rabbitSpawnTime = Time.time;
        }
        else if (currentStage == GrowState.ADULT && ShouldSpawnFruit())
        {
            
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
        ChangeStagePrefab(newStagePrefab);
    }

    private void ChangeStagePrefab(GameObject newStagePrefab)
    {
        Destroy(currentStagePrefab.gameObject);

        GameObject treeStage = Instantiate(newStagePrefab, transform.position, transform.rotation) as GameObject;

		currentStagePrefab = treeStage.transform;
        currentStagePrefab.parent = gameObject.transform;
        currentStagePrefab.transform.up = terrainUp;

        currentStagePrefab.localScale = treeScale;
        currentStagePrefab.transform.Rotate(treeRotation);
	}

    bool ShouldSpawnRabbit()
    {
        return (Time.time - rabbitSpawnTime) > timeTillNextAnimal && !isRabbitSpawned;
    }

    bool ShouldSpawnFruit()
    {
        
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
            treeRotation = new Vector3(0, randomY, 0);

            GrowToNextStage();
		}
	}
}
