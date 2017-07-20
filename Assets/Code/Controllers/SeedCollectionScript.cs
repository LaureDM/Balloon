using UnityEngine;

public class SeedCollectionScript : MonoBehaviour {

    #region Editor Fields

    [SerializeField]
	private GameObject pineTreeSeedPrefab;

	[SerializeField]
	private GameObject oakTreeSeedPrefab;

	[SerializeField]
	private GameObject appleTreeSeedPrefab;

    #endregion

    #region Helper Methods

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

    #endregion
}
