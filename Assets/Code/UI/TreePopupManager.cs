using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePopupManager : MonoBehaviour {

    private TreeScript treeScript;

    public TreeScript TreeScript 
    { 
        get
        {
            return treeScript;
        }
        set 
        {
            treeScript = value;
            InitializeDialog();
        } 
    }

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		//TODO show grow stage duration
	}

    public void InitializeDialog()
    {
        //TODO
    }

    //TODO FOR TESTING
    public void DeleteTree()
    {
        Destroy(gameObject.transform.parent.transform.parent.gameObject);
    }

    public void UpgradeTree()
    {
        //TODO show new popup
    }
}
