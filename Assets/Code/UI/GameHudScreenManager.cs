using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameHudScreenManager : MonoBehaviour
{

    #region Editor Fields

    [SerializeField]
    private Text seedCount;

    [SerializeField]
    private Image seedImage;

    #endregion

    #region Fields

    private InventoryManager inventoryManager;
    private TreeType selectedSeed;
    private CameraDragManager cameraDragManager;
    private SeedDropperScript seedDropper;

    #endregion

    // Use this for initialization
    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        cameraDragManager = FindObjectOfType<CameraDragManager>();
        seedDropper = FindObjectOfType<SeedDropperScript>();

        selectedSeed = inventoryManager.Seeds.First().Key;

        //get the first treetype of the dictionary
        //initialize fields

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Helper Methods

    public void InitializeSeedButton()
    {
        
    }

    #endregion

    #region UI Callbacks

    public void OnRightArrowClicked()
    {
        
    }

    public void OnLeftArrowClicked()
    {
        
    }

    public void OnStartDrag()
    {
        cameraDragManager.Disabled = true;

        //instantiate seed
        seedDropper.InstantiateSeed(selectedSeed);

    }
        
    public void OnSeedDragged()
    {
        Debug.Log("Drag");

        //move seed positions
    }

    public void OnSeedDropped()
    {
        //check area

        Debug.Log("Drop");
        cameraDragManager.Disabled = false;
        seedDropper.DropSeed();
    }

    #endregion
}
