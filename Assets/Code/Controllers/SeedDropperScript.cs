using UnityEngine;

public class SeedDropperScript : MonoBehaviour {

	[SerializeField]
	private GameObject pineTreeSeedPrefab;

	[SerializeField]
	private GameObject oakTreeSeedPrefab;

	[SerializeField]
	private GameObject appleTreeSeedPrefab;

	private SeedDropperScript Instance;

	public void InstantiateSeed(TreeType treeType)
	{
		GameObject prefab = null;

        switch (treeType) 
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
