using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class BalloonController : MonoBehaviour {

	public static string IS_MOVING = "IsMoving";

	[SerializeField]
	private float speed;

	[SerializeField]
	private float tilt;

	[SerializeField]
	private Camera balloonCamera;

	[SerializeField]
	private GameObject floor;

	[SerializeField]
	private SeedDropperScript seedDropper;

	public Animator balloonAnimator;

	private List<TreeType> seeds;

	void Start()
	{
		seeds = new List<TreeType> ();

		seeds.Add (TreeType.OAK);
		seeds.Add (TreeType.OAK);
		seeds.Add (TreeType.OAK);
		seeds.Add (TreeType.APPLE_TREE);
		seeds.Add (TreeType.PINE_TREE);
		seeds.Add (TreeType.PINE_TREE);
	}

	void Update () {
		
		#if UNITY_EDITOR

		float movementHorizontal = Input.GetAxis("Horizontal");
		float movementVertical = Input.GetAxis("Vertical");

		balloonAnimator.SetBool(IS_MOVING, (movementHorizontal != 0 || movementVertical != 0));
			
		Vector3 movement = new Vector3(movementHorizontal, 0.0f, movementVertical);

		GetComponent<Rigidbody>().velocity = movement * speed;

		GetComponent<Rigidbody>().rotation = Quaternion.Euler (GetComponent<Rigidbody>().velocity.z * tilt, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);

		if (Input.GetButtonDown("Jump"))
		{
			seeds.Remove(TreeType.PINE_TREE);
			seedDropper.InstantiateSeed(TreeType.PINE_TREE);
		}

		#else

		if (Input.GetMouseButtonDown (0)) {

		Vector3 position = Input.mousePosition;

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 500.0f)) {
		StartCoroutine (MoveBalloon (hit.transform.position));
		} else {
		Debug.Log("No hit");
		}

		}
		#endif
	}

	/*
	void OnDrawGizmosSelected() {
		Camera camera = GetComponent<Camera>();
		Vector3 p = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(p, 2f);
	}

	private IEnumerator MoveBalloon(Vector3 position) {

		var distance = Mathf.Abs((transform.position - position).magnitude);
		var startPosition = transform.position;
		var t = 0f;

		while (t < 1f) {
			t += Time.deltaTime / distance * speed;
			transform.position = Vector3.Lerp(startPosition, position, t);
			yield return null;
		}
	}
	*/
		
}
