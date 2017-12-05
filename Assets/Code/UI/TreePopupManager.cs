using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreePopupManager : MonoBehaviour {

    #region Serialize Fields
    
    [SerializeField]
    private Text treeName;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Image growStageIcon;

    #endregion

    private TreeScript treeScript;

    public TreeScript TreeScript 
    { 
        get
        {
            return treeScript;
        }
        set 
        {
            treeScript = value;
            InitializeDialog();
        } 
    }

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
        if (TreeScript.IsAdult())
        {
            //TODO display something else
            slider.gameObject.SetActive(false);
            return;
        }
        
        slider.value = TreeScript.GetGrowProgress();
	}

    public void InitializeDialog()
    {
        treeName.text = TreeTypeMapper.GetStringValue(TreeScript.type);
    }

    //TODO FOR TESTING
    public void DeleteTree()
    {
        Destroy(gameObject.transform.parent.transform.parent.gameObject);
    }

    public void UpgradeTree()
    {
        //TODO show new popup
    }
}
