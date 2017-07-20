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
     * The tags of the trees the animal is attracted by
     */
    [SerializeField]
    private string[] treeTags;

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

    #endregion

    #region Fields

    private GameObject terrain;
    private Vector3 currentTarget;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    private bool isResting;
    private bool isEating;
    private bool isGoingTowardsFood;

    private float terrainTargetBoundX;
    private float terrainTargetBoundZ;
    #endregion

    #region Initialization

    void Start()
    {
        terrain = GameObject.FindWithTag("Terrain");

        Bounds bounds = terrain.GetComponent<Collider>().bounds;

        terrainTargetBoundX = bounds.size.x - 5f  * 0.5f;
        terrainTargetBoundZ = bounds.size.z - 5f * 0.5f;

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        FindNewTarget();
    }

    #endregion

    #region Unity Methods

    void Update()
    {
        if (Vector3.Distance(transform.position, navMeshAgent.destination) > 1.0f || isGoingTowardsFood)
        {
            Debug.Log("Going towards target");
            navMeshAgent.SetDestination(currentTarget);
		}
        else
        {
			Debug.Log("target reached");

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
        Debug.Log(collision.collider.gameObject);

        //if animal bumps into fruit he eats it 
        if (collision.collider.gameObject.GetComponent<PineConeScript>())
        {
            //TODO unlock fruit
            //TODO check if fruit is
            StartCoroutine(EatFruit());
            Debug.Log("Ate fruit");
            isGoingTowardsFood = false;
            Destroy(collision.collider.gameObject);
        }
        else if (collision.collider.gameObject.GetComponent<PineTreeScript>())
        {
            Rest();
        }
    }

    #endregion

    #region Helper Methods

    bool CheckTreesOfInterest()
    {
        foreach (string treeTag in treeTags)
        {
            GameObject tree = GameObject.FindWithTag(treeTag);

            if (tree != null)
            {
                return true;
            }
        }

        return false;
    }

    void FindNewTarget()
    {
        //TODO 2 -> find fruit (if he is close to it) WithinRadius
        //TODO lock fruit the moment he finds it so others won't go after it
        //TODO if he has eaten fruit he drops a seed a while later

        Debug.Log("Find new target!");

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 30f);

        foreach (Collider fruit in hitColliders)
        {
            if (fruit.gameObject.GetComponent<PineConeScript>())
            {
                Debug.Log("Found fruit");

                //TODO check if fruit is of interest
                PineConeScript pineCone = fruit.GetComponent<PineConeScript>();

                if (!pineCone.IsLocked)
                {
                    Debug.Log("Found fruit and locked it");
                    pineCone.IsLocked = true;
                    currentTarget = fruit.gameObject.transform.position;
                    isGoingTowardsFood = true;
					navMeshAgent.SetDestination(currentTarget);
					return;
                }
            }
        }

        currentTarget = new Vector3(Random.Range(terrainTargetBoundX, -terrainTargetBoundX), transform.position.y, Random.Range(terrainTargetBoundZ, -terrainTargetBoundZ));

        navMeshAgent.SetDestination(currentTarget);
    }

    #endregion

    #region Coroutines

    IEnumerator Rest()
    {
        if (isResting)
        {
            yield break;
        }

        Debug.Log("Rest"); 

        //TODO play rest animation and wait for it to end
        isResting = true;

        yield return new WaitForSeconds(3);

		FindNewTarget();
		isResting = false;
	}

    IEnumerator EatFruit()
    {
        if (isEating)
        {
            yield break;
        }

        isEating = true;

        yield return new WaitForSeconds(3);

        FindNewTarget();
        isEating = false;
    }

    #endregion
}
