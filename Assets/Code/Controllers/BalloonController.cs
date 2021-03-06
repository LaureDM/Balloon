﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class BalloonController : MonoBehaviour {

    #region Editor Fields

    [SerializeField]
	private float speed;

	[SerializeField]
	private float tilt;

	[SerializeField]
	private SeedDropperScript seedDropper;

    [SerializeField]
    private InventoryManager inventoryManager;

    #endregion

    #region Fields

    public Animator balloonAnimator;

    #endregion

    #region Initialization

    #endregion

    #region Unity Methods

    void Update () {
		
		float movementHorizontal = Input.GetAxis(InputValues.HORIZONTAL);
		float movementVertical = Input.GetAxis(InputValues.VERTICAL);

		balloonAnimator.SetBool(AnimatorParameters.IS_MOVING, (movementHorizontal != 0 || movementVertical != 0));
        			
		Vector3 movement = new Vector3(movementHorizontal, 0.0f, movementVertical);

		GetComponent<Rigidbody>().velocity = movement * speed;

		GetComponent<Rigidbody>().rotation = Quaternion.Euler (GetComponent<Rigidbody>().velocity.z * tilt, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);

		if (Input.GetButtonDown(InputValues.JUMP))
		{
            if (inventoryManager.CanInstantiateSeed(TreeType.PINE_TREE))
            {
                seedDropper.InstantiateSeed(TreeType.PINE_TREE);
            }
		}
	}

    #endregion

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
