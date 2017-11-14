using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineTreeSeedScript : MonoBehaviour {

	#region Editor Fields

	[SerializeField]
	private Projector projector;

	[SerializeField]
	private Material materialGreen;

	[SerializeField]
	private Material materialRed;
	
	#endregion

	// Use this for initialization
	void Start () 
	{
		SetIsSafeToDrop(true);
	}

	public void SetIsSafeToDrop(bool safe)
	{
		Material material = safe ? materialGreen : materialRed;
		projector.material = material;
	}

	public void TurnOffProjector()
	{
		projector.gameObject.SetActive(false);
	}

}
