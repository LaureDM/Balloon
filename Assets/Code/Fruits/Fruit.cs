using UnityEngine;
using System.Collections;
using AssemblyCSharp.Code.Enums;

public class Fruit : MonoBehaviour {

    #region Delegates

    public delegate void EatAction(GameObject fruit);
    public event EatAction OnEaten;

    #endregion

    #region 

    public FruitType FruitType;

    #endregion
    #region Helper Methods

    public void SetEaten()
    {
        if (OnEaten != null && gameObject != null)
        {
            OnEaten(gameObject);
            Destroy(gameObject);
        }
    }

    public void OnDestroy()
    {
        //remove subscribers but how
    }

    #endregion
}
