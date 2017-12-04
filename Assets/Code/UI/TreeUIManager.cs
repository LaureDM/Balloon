using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeUIManager : MonoBehaviour {


	[SerializeField]
	private GameObject treePopupPrefab;

	// Use this for initialization
	void Start () 
	{
		gameObject.SetActive(false);
		Instantiate(treePopupPrefab, transform.position, transform.rotation, gameObject.transform);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void ShowPopup(TreeScript tree)
	{
		//treePopup.SetTree(tree);
		gameObject.SetActive(true);
	}

	public void HidePopup()
	{
		gameObject.SetActive(false);
	}
}
