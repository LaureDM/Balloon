using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{

    #region Delegates

    public delegate void CountAction(TreeType type);
    public event CountAction OnCountChanged;

    #endregion

    public Dictionary<TreeType, int> Seeds { get; set; }

    private void Awake()
    {
        //THIS SHOULD BE FETCHED FROM DB
        Seeds = new Dictionary<TreeType, int>();

        //TODO TEST CODE
        Seeds.Add(TreeType.PINE_TREE, 3);
    }

    // Use this for initialization
    void Start()
	{
        
	}

    public void IncreaseSeedCount(TreeType seed)
    {
        if (Seeds.ContainsKey(seed))
        {
            Seeds[seed] = Seeds[seed] + 1;
        }
        else
        {
            Seeds.Add(seed, 1);
        }

        if (OnCountChanged != null)
        {
            OnCountChanged(seed);
        }
    }

    public bool CanInstantiateSeed(TreeType seed)
    {
        int currentCount = 0;

        if (Seeds.ContainsKey(seed) && Seeds.TryGetValue(seed, out currentCount))
        {
            if (currentCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //seed does not exist in the list so return false
        return false;
    }

    public void DecreaseSeedCount(TreeType seed)
    {
        int currentCount = 0;
        Seeds.TryGetValue(seed, out currentCount);
        Seeds[seed] = currentCount - 1;
        if (OnCountChanged != null)
        {
            OnCountChanged(seed);
        }
    }

}
