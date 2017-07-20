using UnityEngine;

public class CameraController : MonoBehaviour {

    #region Editor Fields

    [SerializeField]
	private GameObject player;

    #endregion

    #region Fields

    private Vector3 offset;
	private float cameraHeight;

    #endregion

    #region Initialization

    // Use this for initialization
    void Start () 
	{
		offset = transform.position - player.transform.position;
		cameraHeight = transform.position.y;
	}

    #endregion

    #region Unity Methods

    public void LateUpdate()
	{
		Vector3 balloonPosition = player.transform.position + offset;
		transform.position = new Vector3 (balloonPosition.x, cameraHeight, balloonPosition.z);
	}

    #endregion
}
