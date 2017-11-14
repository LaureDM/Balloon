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

    [SerializeField]
    private Button seedButton;

    [SerializeField]
    private Image seedButtonIcon;

    [SerializeField]
    private Sprite seedIcon;

    [SerializeField]
    private Sprite cancelIcon;

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

        InitializeSeedButton();

        //subscribe to inventory events
        inventoryManager.OnCountChanged += OnCountChanged;

        seedButtonIcon.sprite = seedIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Helper Methods

    public void InitializeSeedButton()
    {
        //TODO change image
        UpdateCount(selectedSeed);
        CheckInteractable();
    }

    private void UpdateCount(TreeType type)
    {
        int count = 0;
        inventoryManager.Seeds.TryGetValue(type, out count);
        seedCount.text = count.ToString();
    }

    private void CheckInteractable()
    {
        if (inventoryManager.CanInstantiateSeed(selectedSeed))
        {
            seedButton.interactable = true;
        }
        else
        {
            seedButton.interactable = false;
        }
    }

    #endregion

    #region Event Callbacks

    public void OnCountChanged(TreeType type)
    {
        if (selectedSeed == type)
        {
            InitializeSeedButton();            
        }

        //TODO
        //else display add of seed?
    }

    #endregion

    #region UI Callbacks

    public void OnRightArrowClicked()
    {
        //TODO
    }

    public void OnLeftArrowClicked()
    {
        //TODO
    }

    public void OnStartDrag()
    {
        if (inventoryManager.CanInstantiateSeed(selectedSeed))
        {
            cameraDragManager.Disabled = true;
            seedDropper.InstantiateSeed(selectedSeed);

            seedButtonIcon.sprite = cancelIcon;
        }
    }

    public void OnSeedDropped()
    {
        cameraDragManager.Disabled = false;

        seedButtonIcon.sprite = seedIcon;
        
        seedDropper.DropSeed();
    }

    public void OnCancelButtonClicked()
    {
        seedDropper.DestroySeed();
        seedButtonIcon.sprite = seedIcon;
    }

    #endregion
}
