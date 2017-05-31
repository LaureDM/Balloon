using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField]
	private GameObject player;

	private Vector3 offset;
	private float cameraHeight;

	// Use this for initialization
	void Start () 
	{
		offset = transform.position - player.transform.position;
		cameraHeight = transform.position.y;
	}

	public void LateUpdate()
	{
		Vector3 balloonPosition = player.transform.position + offset;
		transform.position = new Vector3 (balloonPosition.x, cameraHeight, balloonPosition.z);
	}
}
