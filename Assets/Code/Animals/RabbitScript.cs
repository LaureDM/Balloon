﻿﻿using System.Collections;
using UnityEngine;

public class RabbitScript : MonoBehaviour
{
    #region Editor Fields

    /*
     * The fruits this animal can eat
     */
    [SerializeField]
    private GameObject[] fruits;


    /*
     * The types of trees the animal is attracted by
     */
    [SerializeField]
    private TreeType[] treeTypes;

    /*
     * The seeds the animal can drop
     */
    [SerializeField]
    private GameObject[] seeds;
    
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

    private GameObject terrain;
    private Vector3 currentTarget;
    private PineConeScript currentFruitTarget;

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
        terrain = GameObject.FindWithTag("Terrain");

        Bounds bounds = terrain.GetComponent<Collider>().bounds;

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
        Transform parent = bumpedObject.transform.parent;

        //if animal bumps into fruit he eats it 
        if (parent != null && parent.gameObject.GetComponent<PineConeScript>())
        {
            //TODO check if fruit is fruit he wants
            ateFruit = true;
            PineConeScript pineCone = parent.gameObject.GetComponent<PineConeScript>();
            isGoingTowardsFood = false;
            StartCoroutine(EatFruit(pineCone));
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
            if (parent != null && parent.gameObject.GetComponent<PineConeScript>())
            {
                //TODO check if fruit is of interest
                PineConeScript pineCone = parent.GetComponent<PineConeScript>();

                //subscribe to event
                pineCone.OnEaten += OnFruitEaten;

                currentTarget = parent.transform.position;
                currentFruitTarget = pineCone;

                isGoingTowardsFood = true;
			    navMeshAgent.SetDestination(currentTarget);
				return;
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
        PineConeScript pineCone = fruit.GetComponent<PineConeScript>();
        pineCone.OnEaten -= OnFruitEaten;
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

        //TODO calculate random seed
        StartCoroutine(seedDropEffectScript.CreateEffect());

        inventoryManager.IncreaseSeedCount(TreeType.PINE_TREE);

        UnPauseNavMeshAgent();
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

    IEnumerator EatFruit(PineConeScript pineCone)
    {
        if (isEating)
        {
            yield break;
        }

        //TODO play eat animation and wait for it to end
        isEating = true;

        PauseNavMeshAgent();

        yield return new WaitForSeconds(3);

        pineCone.SetEaten();

        isEating = false;
    }

    #endregion
}
