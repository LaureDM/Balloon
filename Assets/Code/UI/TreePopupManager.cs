using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePopupManager : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void SetTree(TreeScript tree)
    {

    }

    //TODO FOR TESTING
    public void DeleteTree()
    {
        Destroy(gameObject.transform.parent.transform.parent.gameObject);
    }
}
