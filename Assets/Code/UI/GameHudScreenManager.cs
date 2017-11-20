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

    [SerializeField]
    private GameObject counter;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Button previousButton;

    [SerializeField]
    private Text selectedTreeText;

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

    void Update()
    {
        //hide or show arrow keys
        if (inventoryManager.Seeds.First().Key == selectedSeed)
        {
            previousButton.gameObject.SetActive(false);
        }
        else
        {
            previousButton.gameObject.SetActive(true);
        }

        if (inventoryManager.Seeds.Last().Key == selectedSeed)
        {
            nextButton.gameObject.SetActive(false);
        }
        else
        {
            nextButton.gameObject.SetActive(true);
        }
    }

    #region Helper Methods

    public void InitializeSeedButton()
    {
        //TODO change image
        selectedTreeText.text = TreeTypeMapper.GetStringValue(selectedSeed);
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
        selectedSeed = inventoryManager.Seeds.Keys.ToList()[inventoryManager.GetIndexOfKey(selectedSeed) + 1];
        InitializeSeedButton();
    }

    public void OnLeftArrowClicked()
    {
        selectedSeed = inventoryManager.Seeds.Keys.ToList()[inventoryManager.GetIndexOfKey(selectedSeed) - 1];
        InitializeSeedButton();
    }

    public void OnStartDrag()
    {
        if (inventoryManager.CanInstantiateSeed(selectedSeed))
        {
            cameraDragManager.Disabled = true;
            seedDropper.InstantiateSeed(selectedSeed);

            seedButtonIcon.sprite = cancelIcon;
            counter.SetActive(false);
        }
    }

    public void OnSeedDropped()
    {
        cameraDragManager.Disabled = false;

        seedButtonIcon.sprite = seedIcon;
        counter.SetActive(true);
        
        seedDropper.DropSeed();
    }

    public void OnCancelButtonClicked()
    {
        seedDropper.DestroySeed();
        seedButtonIcon.sprite = seedIcon;
        counter.SetActive(true);
    }

    #endregion
}
