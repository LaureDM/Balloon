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

    private PineTreeSeedScript seedScript;
    private bool isDragging;
    private bool isSafeToDrop;
    private InventoryManager inventoryManager;
    private TreeType instantiatedTreeType;

    #endregion

    #region Unity Methods

    public void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void Update()
    {
        //get touch input and get mouse input
        if (isDragging && instantiatedSeed != null)
        {
            //screen?
            Vector3 mousePosition = Input.mousePosition;
            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out hit)) 
            {
                //collider is the terrain, safe to drop seed
                if (hit.collider.gameObject.GetComponent<TerrainScript>())
                {
                    isSafeToDrop = true;
                }
                else
                {
                    isSafeToDrop = false;
                }

                Vector3 seedPosition = new Vector3(hit.point.x, hit.point.y + 10, hit.point.z);
                instantiatedSeed.transform.position = seedPosition;
                seedScript.SetIsSafeToDrop(isSafeToDrop);
            }
            //seed dragging is out of bounds, cancel the seed drag
            else
            {
                DestroySeed();
            }
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
        seedScript = instantiatedSeed.GetComponentInChildren<PineTreeSeedScript>();
        instantiatedTreeType = treeType;

        instantiatedSeed.transform.parent = transform;
        isDragging = true;
    }

    public void DropSeed()
    {
        if (isSafeToDrop && instantiatedSeed != null)
        {
            seedScript.TurnOffProjector();
            isDragging = false;
            inventoryManager.DecreaseSeedCount(instantiatedTreeType);
        }
        else
        {
            DestroySeed();
        }
    }

    public void DestroySeed()
    {
        if (instantiatedSeed != null)
        {
            isDragging = false;
            Destroy(instantiatedSeed);
            instantiatedSeed = null;
        }
    }

    #endregion
}
