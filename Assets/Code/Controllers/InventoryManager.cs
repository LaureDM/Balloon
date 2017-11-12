using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
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
    }


    public bool DecreaseSeedCount(TreeType seed)
    {
        int currentCount = 0;

        if (Seeds.ContainsKey(seed) && Seeds.TryGetValue(seed, out currentCount))
        {
            if (currentCount == 0)
            {
                //return false because you can not instantiate seed
                return false;
            }
            else
            {
                Seeds[seed] = currentCount - 1;
                return true;
            }
        }

        return false;
    }

}
