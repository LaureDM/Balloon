﻿﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssemblyCSharp.Code.Controllers;
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

    [SerializeField]
    private Animator animalController;

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
    private float timeTillAnimalWillLeave;
    private InventoryManager inventoryManager;
    private TreeCollectionScript treeCollection;

    private AnimalCollectionScript animalCollection;

    private bool isLeaving;

    private bool areTreesGone;

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
        timeTillAnimalWillLeave = durationWithoutTrees;

        inventoryManager = FindObjectOfType<InventoryManager>();
        treeCollection = FindObjectOfType<TreeCollectionScript>();
        animalCollection = FindObjectOfType<AnimalCollectionScript>();

        //subscribe to tree collection to know about tree sort gone/restored events
        treeCollection.OnTreeSortGone += OnTreeSortGone;
        treeCollection.OnTreeSortRestored += OnTreeSortRestored;

        animalController.SetBool(AnimatorParameters.IS_WALKING, false);

        FindNewTarget();
    }

    #endregion

    #region Unity Methods

    void Update()
    {
        if (areTreesGone)
        {
            timeTillAnimalWillLeave -= Time.deltaTime;
        }

        if (ateFruit)
        {
            currentTimeTillSeedDrops -= Time.deltaTime;
        }

        if (currentTimeTillSeedDrops <= 0)
        {
            ateFruit = false;
            currentTimeTillSeedDrops = timeTillSeedDrops;
            LeaveSeed();
        }

        if (timeTillAnimalWillLeave <= 0 && !isLeaving)
        {
            Leave();
        }

        if (Vector3.Distance(transform.position, navMeshAgent.destination) > 1.0f || isGoingTowardsFood)
        {
            //let animal keep walking towards target
            return;
		} 
        //animal was leaving and will disappear
        else if (isLeaving)
        {
            Destroy(gameObject);
        }
        //animal reached target and will rest
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
        //unsubscribe from all events

        if (currentFruitTarget != null)
        {
            currentFruitTarget.OnEaten -= OnFruitEaten;
        }   

        treeCollection.OnTreeSortGone -= OnTreeSortGone;
        treeCollection.OnTreeSortRestored -= OnTreeSortRestored;
    }

    public void OnDisable()
    {
        //unsubscribe from all events

        if (currentFruitTarget != null)
        {
            currentFruitTarget.OnEaten -= OnFruitEaten;
        }
        
        treeCollection.OnTreeSortGone -= OnTreeSortGone;
        treeCollection.OnTreeSortRestored -= OnTreeSortRestored;
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

        animalController.SetBool(AnimatorParameters.IS_WALKING, false);

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
    There are no more trees so animal leaves
    */
    public void Leave()
    {
        isLeaving = true;
        currentTarget = animalCollection.gameObject.transform.position;
    }

    private bool CheckTreeOfInterest(TreeType type)
    {
        return treeTypes.Contains(type);
    }

    #region Events

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

    public void OnTreeSortGone(TreeType type)
    {
        foreach (TreeType tree in treeTypes)
        {
            if (!treeCollection.GoneTrees.Contains(tree))
            {
                return;
            }
        }    

        //all tree types animal is attracted to are gone, his countdown will start
        areTreesGone = true;
    }

    public void OnTreeSortRestored(TreeType type)
    {
        //a tree the animal is attracted to is restored
        if (CheckTreeOfInterest(type))
        {
            //animal will stay in the forest
            if (isLeaving)
            {   
                isLeaving = false;
                areTreesGone = false;
                timeTillAnimalWillLeave = durationWithoutTrees;
                FindNewTarget();
            }
        }
    }

    #endregion

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

        animalController.SetBool(AnimatorParameters.IS_WALKING, false);
        //TODO play rest animation and wait for it to end
        isResting = true;

        PauseNavMeshAgent();

        yield return new WaitForSeconds(3);

		FindNewTarget();

        animalController.SetBool(AnimatorParameters.IS_WALKING, true);
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

        if (fruit != null)
        {
            fruit.SetEaten();
        }

        isEating = false;
    }

    #endregion
}
