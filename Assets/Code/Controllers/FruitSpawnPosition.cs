using AssemblyCSharp.Code.Enums;
using UnityEngine;

namespace AssemblyCSharp.Code.Controllers
{
    public class FruitSpawnPosition : MonoBehaviour
    {
		[SerializeField]
		private GameObject pineconePrefab;

		[SerializeField]
		private GameObject applePrefab;

        public void SpawnFruit(FruitType fruitType)
        {
            GameObject prefab = null;

            switch (fruitType)
            {
                case FruitType.APPLE:
                    prefab = applePrefab;
                    break;

                case FruitType.PINECONE:
                    prefab = pineconePrefab;
                    break;
            }

            GameObject fruit = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            fruit.transform.parent = gameObject.transform;
        }

        public bool IsFruitSpawned()
        {
            return transform.childCount > 0;
        }
    }
}
