using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp.Code.Controllers
{
    public class TreeCollectionScript : MonoBehaviour
    {
        #region Events

        public delegate void TreeSortGoneEvent(TreeType type);
        public event TreeSortGoneEvent OnTreeSortGone;

        public delegate void TreeSortRestoredEvent(TreeType type);
        public event TreeSortRestoredEvent OnTreeSortRestored;


        #endregion

        #region Fields

        /*
        *  Discovered trees with their count in the forest
        */
        private Dictionary<TreeType, int> treeDictionary; 

        /*
        * Adult trees in the forest
        */       
        private Dictionary<TreeType, int> adultTreeDictionary; 

        #endregion

        #region Properties

        public List<TreeType> GoneTrees { get; set; }

        #endregion

        public void IncreaseTreeCount(TreeType tree)
        {
            if (treeDictionary.ContainsKey(tree))
            {
                treeDictionary[tree] = treeDictionary[tree] + 1;
            }
            else
            {
                treeDictionary.Add(tree, 1);
            }
        }

        public void IncreaseAdultTreeCount(TreeType tree)
        {
            int currentCount = 0;

            if (adultTreeDictionary.ContainsKey(tree) && adultTreeDictionary.TryGetValue(tree, out currentCount))
            {
                //tree sort is restored
                if (currentCount == 0)
                {
                    if (OnTreeSortRestored != null)
                    {
                        GoneTrees.Remove(tree);
                        OnTreeSortRestored(tree);
                    }
                }

                adultTreeDictionary[tree] = currentCount + 1;
            }
            else
            {
                treeDictionary.Add(tree, 1);
            }
        }

        public void DecreaseTreeCount(TreeType treeType, GameObject tree)
        {
            if (treeDictionary.ContainsKey(treeType))
            {
                int currentCount = treeDictionary[treeType];
                treeDictionary[treeType] = currentCount - 1 < 0 ? 0 : currentCount - 1;
            }

            if (tree)
            {
                TreeScript treeScript = tree.GetComponent<TreeScript>();
                if (treeScript.IsAdult())
                {
                    DecreaseAdultTreeCount(treeType);
                }
            }
        }

        private void DecreaseAdultTreeCount(TreeType tree)
        {
            if (adultTreeDictionary.ContainsKey(tree))
            {
                int currentCount = adultTreeDictionary[tree];

                //last tree
                if (currentCount == 1)
                {
                    adultTreeDictionary[tree] = 0;

                    if (OnTreeSortGone != null)
                    {
                        GoneTrees.Add(tree);
                        OnTreeSortGone(tree);
                    }
                }
                //is this even possible?
                else if (currentCount == 0)
                {
                    adultTreeDictionary[tree] = 0;
                }
                else 
                {
                    adultTreeDictionary[tree] = currentCount - 1; 
                }
            }
        }
    }
}
