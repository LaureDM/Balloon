using UnityEngine;

public class SeedDropperScript : MonoBehaviour {

    #region Editor Fields

    [SerializeField]
    private GameObject pineTreeSeedPrefab;

    [SerializeField]
    private GameObject oakTreeSeedPrefab;

    [SerializeField]
    private GameObject appleTreeSeedPrefab;

    private GameObject instantiatedSeed;

    #endregion

    #region Unity Methods

    public void Update()
    {
        //get touch input and get mouse input
        if (Input.GetMouseButtonDown(0))
        {
            //screen?
            instantiatedSeed.transform.position = Input.mousePosition;
        }

    }

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

        instantiatedSeed = Instantiate (prefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        instantiatedSeed.transform.parent = transform;
    }

    public void DropSeed()
    {
        
    }

    public void DestroySeed()
    {
        
    }

    #endregion
}
