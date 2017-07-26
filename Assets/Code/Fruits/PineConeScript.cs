using UnityEngine;
using System.Collections;

public class PineConeScript : MonoBehaviour {

    #region Delegates

    public delegate void EatAction(GameObject fruit);
    public event EatAction OnEaten;

    #endregion

    #region Helper Methods

    public void SetEaten()
    {
        if (OnEaten != null)
        {
            OnEaten(gameObject);
            Destroy(gameObject);
        }
    }

    #endregion
}
