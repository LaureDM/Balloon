﻿﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssemblyCSharp.Code.Enums;
using UnityEngine;

public class AnimalScript : MonoBehaviour
{
    #region Editor Fields

    /*
     * The fruits this animal eats
     */
    [SerializeField]
    private List<FruitType> fruits;


    /*
     * The types of trees the animal is attracted by
     */
    [SerializeField]
    private List<TreeType> treeTypes;

    /*
     * The seeds the animal can drop
     */
    [SerializeField]
    private SeedPercentageDictionary seeds;
    
    /*
     * Indicates how long an animal can survive without trees
     */
    [SerializeField]
    private float durationWithoutTrees;

    [SerializeField]
    private float timeTillSeedDrops;

    [SerializeField]
    private SeedDropEffectScript seedDropEffectScript;

    #endregion

    #region Fields

    private TerrainScript terrain;
    private Vector3 currentTarget;
    private Fruit currentFruitTarget;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    private bool isResting;
    private bool isEating;
    private bool isGoingTowardsFood;

    private float terrainTargetBoundX;
    private float terrainTargetBoundZ;

    private bool ateFruit;

    private float currentTimeTillSeedDrops;
    private InventoryManager inventoryManager;

    #endregion

    #region Initialization

    void Start()
    {
        terrain = FindObjectOfType<TerrainScript>();

        Bounds bounds = terrain.gameObject.GetComponent<Collider>().bounds;

        terrainTargetBoundX = (bounds.size.x - 5f)/2;
        terrainTargetBoundZ = (bounds.size.z - 5f)/2;

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        currentTimeTillSeedDrops = timeTillSeedDrops;

        inventoryManager = FindObjectOfType<InventoryManager>();
        FindNewTarget();
    }

    #endregion

    #region Unity Methods

    void Update()
    {
        if (currentTimeTillSeedDrops <= 0)
        {
            ateFruit = false;
            currentTimeTillSeedDrops = timeTillSeedDrops;
            LeaveSeed();
        }

        if (ateFruit)
        {
            currentTimeTillSeedDrops -= Time.deltaTime;
        }

        if (Vector3.Distance(transform.position, navMeshAgent.destination) > 1.0f || isGoingTowardsFood)
        {
            navMeshAgent.SetDestination(currentTarget);
		} 
        else 
        {
			StartCoroutine(Rest());
		}

        //TODO find trees -> duration is restored
        //TODO find fruits -> when he eats fruit he drops seed a random time later
        //TODO walk around
        //TODO durationWithoutTrees is counting down when there are no more trees
        //TODO if 0, animal dies
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject bumpedObject = collision.collider.gameObject;
        Fruit fruitScript = bumpedObject.GetComponentInParent<Fruit>();

        //if animal bumps into fruit he eats it 
        if (fruitScript != null)
        {
            //check if fruit is fruit he wants
            if (!fruits.Contains(fruitScript.FruitType))
            {
                return;
            }

            ateFruit = true;
            isGoingTowardsFood = false;
            StartCoroutine(EatFruit(fruitScript));
        }
        else
        {
            StartCoroutine(Rest());
        }
    }

    public void OnDestroy()
    {
        if (currentFruitTarget != null)
        {
            currentFruitTarget.OnEaten -= OnFruitEaten;
        }
    }

    public void OnDisable()
    {
        if (currentFruitTarget != null)
        {
            currentFruitTarget.OnEaten -= OnFruitEaten;
        }
    }

    #endregion

    #region Helper Methods

    void FindNewTarget()
    {
        //TODO 2 -> find fruit (if he is close to it) WithinRadius
        //TODO lock fruit the moment he finds it so others won't go after it
        //TODO if he has eaten fruit he drops a seed a while later
        //No need to look for a fruit target if he just ate one
        if (ateFruit)
        {
            currentTarget = new Vector3(Random.Range(terrainTargetBoundX, -terrainTargetBoundX), transform.position.y, Random.Range(terrainTargetBoundZ, -terrainTargetBoundZ));
            navMeshAgent.SetDestination(currentTarget);
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 30f);

        foreach (Collider fruit in hitColliders)
        {
            Transform parent = fruit.transform.parent;
            Fruit fruitScript = fruit.GetComponentInParent<Fruit>();
            if (fruitScript != null)
            {
                //check if fruit is of interest
                if (fruits.Contains(fruitScript.FruitType))
                {
                    //subscribe to event
                    fruitScript.OnEaten += OnFruitEaten;

                    currentTarget = parent.transform.position;
                    currentFruitTarget = fruitScript;

                    isGoingTowardsFood = true;
                    navMeshAgent.SetDestination(currentTarget);
                    return;
                    }            
            }
        }

        currentTarget = new Vector3(Random.Range(terrainTargetBoundX, -terrainTargetBoundX), transform.position.y, Random.Range(terrainTargetBoundZ, -terrainTargetBoundZ));
        navMeshAgent.SetDestination(currentTarget);
    }

    /*
     * Subscription method when fruit eaten event is called
     */
    public void OnFruitEaten(GameObject fruit)
    {
        Fruit fruitScript = fruit.GetComponent<Fruit>();
        fruitScript.OnEaten -= OnFruitEaten;
        currentFruitTarget = null;
        FindNewTarget();
    }

    public void PauseNavMeshAgent()
    {
        navMeshAgent.destination = transform.position;
    }

    public void UnPauseNavMeshAgent()
    {
        navMeshAgent.destination = currentTarget;
    }

    public void LeaveSeed()
    {
        PauseNavMeshAgent();

        StartCoroutine(seedDropEffectScript.CreateEffect());

        TreeType type = CalculateSeed();

        inventoryManager.IncreaseSeedCount(type);

        UnPauseNavMeshAgent();
    }

    public TreeType CalculateSeed()
    {
        int randomNumber = Random.Range(0, 100);
        int percentage = 0;

        foreach (KeyValuePair<TreeType, int> entry in seeds)
        {
            percentage += entry.Value;

            if (randomNumber <= percentage)
            {
                return entry.Key;
            }
        }

        //if for some reason something goes wrong, return the first animal possible
        return seeds.First().Key;    
    }

    #endregion

    #region Coroutines

    IEnumerator Rest()
    {
        if (isResting)
        {
            yield break;
        }

        //TODO play rest animation and wait for it to end
        isResting = true;

        PauseNavMeshAgent();

        yield return new WaitForSeconds(3);

		FindNewTarget();
		isResting = false;
	}

    IEnumerator EatFruit(Fruit fruit)
    {
        if (isEating)
        {
            yield break;
        }

        //TODO play eat animation and wait for it to end
        isEating = true;

        PauseNavMeshAgent();

        yield return new WaitForSeconds(3);

        fruit.SetEaten();

        isEating = false;
    }

    #endregion
}
