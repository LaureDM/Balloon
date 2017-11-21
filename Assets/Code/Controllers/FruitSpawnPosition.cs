using AssemblyCSharp.Code.Enums;
using UnityEngine;

namespace AssemblyCSharp.Code.Controllers
{
    public class FruitSpawnPosition : MonoBehaviour
    {
        #region Editor Fields

        [SerializeField]
		private GameObject fruitPrefab;

        #endregion

        #region Helper Methods

        public void SpawnFruit()
        {
            GameObject fruit = Instantiate(fruitPrefab, transform.position, transform.rotation) as GameObject;
            fruit.transform.parent = gameObject.transform;
        }

        public bool IsFruitSpawned()
        {
            return transform.childCount > 0;
        }

        #endregion
    }
}
