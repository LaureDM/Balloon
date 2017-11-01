using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<TreeType, int> seeds;

	// Use this for initialization
	void Start()
	{
        //THIS SHOULD BE FETCHED FROM DB
        seeds = new Dictionary<TreeType, int>();

        //TODO TEST CODE
        seeds.Add(TreeType.PINE_TREE, 3);
	}

    public void IncreaseSeedCount(TreeType seed)
    {
        if (seeds.ContainsKey(seed))
        {
            seeds[seed] = seeds[seed] + 1;
        }
        else
        {
            seeds.Add(seed, 1);
        }
    }


    public bool DecreaseSeedCount(TreeType seed)
    {
        int currentCount = 0;

        if (seeds.ContainsKey(seed) && seeds.TryGetValue(seed, out currentCount))
        {
            if (currentCount == 0)
            {
                //return false because you can not instantiate seed
                return false;
            }
            else
            {
                seeds[seed] = currentCount - 1;
                return true;
            }
        }

        return false;
    }

}
