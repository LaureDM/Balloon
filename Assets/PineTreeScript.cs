﻿using UnityEngine;
using System.Collections;
using AssemblyCSharp.Code.Enums;

public class PineTreeScript : MonoBehaviour {

	[SerializeField]
	private GameObject[] growStages;

    [SerializeField]
    private Rigidbody rigidBody;

    private Transform currentStagePrefab;
    private GrowState currentStage = GrowState.SEED;

	void Start () 
    {
        currentStagePrefab = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (currentStage != GrowState.SEED)
        {
            //TODO start counting time
        }
	}

	public void OnTriggerEnter(Collider collider)
	{
        //seed hits the ground
        if (collider.gameObject.GetComponent<TerrainScript>() != null)
        {
            rigidBody.isKinematic = false;
            GrowToNextStage();
        }
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

            default:
            case GrowState.SAPLING:

                currentStage = GrowState.ADULT;
                newStagePrefab = growStages[2];
                break;
        }

        ChangeStagePrefab(newStagePrefab);
    }

    private void ChangeStagePrefab(GameObject newStagePrefab)
    {
		Destroy(currentStagePrefab.gameObject);

        GameObject treeStage = Instantiate(newStagePrefab, transform.position, transform.rotation) as GameObject;
		currentStagePrefab = treeStage.transform;
        currentStagePrefab.parent = gameObject.transform;
	}
}
