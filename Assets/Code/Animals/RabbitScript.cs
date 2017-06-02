﻿using System.Collections;
using UnityEngine;

public class RabbitScript : MonoBehaviour {

    [SerializeField]
    private GameObject[] fruits;

    [SerializeField]
    private string[] treeTags;

    [SerializeField]
    private GameObject[] seeds;

    //this indicates how many seconds they can survive without trees
    [SerializeField]
    private float durationWithoutTrees;

    private GameObject terrain;

    private Vector3 currentTarget;

    private float currentDuration;

    private Rigidbody rigidBody;

    private bool isResting;

    private float minX;
    private float minZ;

    void Start () 
    {
		terrain = GameObject.FindWithTag("Terrain");

		Bounds bounds = terrain.GetComponent<Collider>().bounds;

		minX = bounds.size.x * 0.5f;
		minZ = bounds.size.z * 0.5f;

		FindNewTarget();

        rigidBody = GetComponent<Rigidbody>();
    }
    
    void Update () {

        if (isResting)
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
        }

        if (Vector3.Distance(transform.position, currentTarget) > 1)
        {
            Vector3 dir = (currentTarget - transform.position).normalized * 10f;
			rigidBody.velocity = dir;
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

    void FindNewTarget()
    {
        //TODO random vector3 in bounds
        //TODO 2 -> find fruit (if he is close to it) WithinRadius
        //TODO lock fruit the moment he founds it so others won't go after it
        //TODO if he has eaten fruit he drops a seed a while later

        currentTarget = new Vector3(Random.Range(minX, -minX), transform.position.y, Random.Range(minZ, -minZ));
    }

    IEnumerator Rest()
    {
        if (isResting)
        {
            yield break;
        }

        //TODO play rest animation and wait for it to end
        isResting = true;

        yield return new WaitForSeconds(3);

		FindNewTarget();
		isResting = false;
	}

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

    private void OnCollisionEnter(Collision collision)
    {
        //when rabbit bumps into tree, find a new target to walk to
        if (collision.gameObject.GetComponent<PineTreeScript>())
        {
			FindNewTarget();
		}
    }
}
