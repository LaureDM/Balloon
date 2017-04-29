using UnityEngine;
using System.Collections;

public class SeedDropperScript : MonoBehaviour {

	[SerializeField]
	private GameObject pineTreeSeedPrefab;

	[SerializeField]
	private GameObject oakTreeSeedPrefab;

	[SerializeField]
	private GameObject appleTreeSeedPrefab;

	private SeedDropperScript Instance;



	public void InstantiateSeed(TreeType seedType)
	{
		GameObject prefab = null;

		switch (seedType) 
		{
			case TreeType.APPLE_TREE:
				prefab = appleTreeSeedPrefab;
				break;

			case TreeType.OAK:
				prefab = oakTreeSeedPrefab;
				break;

			case TreeType.PINE_TREE:
				prefab = pineTreeSeedPrefab;
				break;
		}

		Instantiate (prefab, gameObject.transform.position, gameObject.transform.rotation);
	}
}
