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
		GameObject treePopup = Instantiate(treePopupPrefab, transform.position, transform.rotation, gameObject.transform);
		TreeScript treeScript = GetComponentInParent<TreeScript>();
		TreePopupManager treePopupManager = treePopup.GetComponent<TreePopupManager>();
		treePopupManager.TreeScript = treeScript;
	}

	public void ShowPopup()
	{
		gameObject.SetActive(true);
	}

	public void HidePopup()
	{
		gameObject.SetActive(false);
	}
}
