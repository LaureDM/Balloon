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

    [SerializeField]
    private Transform terrain;

    private Vector3 currentTarget;

    private float currentDuration;

    void Start () 
    {
		FindNewTarget();
    }
    
    void Update () {

        if (Vector3.Distance(transform.position, currentTarget) > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, 10f * Time.deltaTime);
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
        //TODO 2 -> find fruit
        //TODO lock fruit the moment he founds it so others won't go after it
        //TODO if he has eaten fruit he drops a seed a while later

        currentTarget = new Vector3(Random.Range(0, 50), transform.position.y, Random.Range(0, 50));
    }

    IEnumerator Rest()
    {
        //TODO play rest animation and wait for it to end
        yield return new WaitForSeconds(3);
		FindNewTarget();

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
}
