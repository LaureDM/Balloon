using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetectionScript : MonoBehaviour {

    private TreeScript currentTreeDisplayingPopup;

    private Vector3 clickPositionStart;
    private Vector3 clickPositionEnd;
	
	// Update is called once per frame
	void Update () 
	{
        //detect tap
		if (Input.GetMouseButtonDown(0))
        {
            clickPositionStart = Input.mousePosition;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            clickPositionEnd = Input.mousePosition;

            //tap
            if (clickPositionStart == clickPositionEnd)
            {
                if (currentTreeDisplayingPopup != null)
                {
                    currentTreeDisplayingPopup.HideTreePopup();
                    currentTreeDisplayingPopup = null;
                    return;
                }

                Vector3 position = Input.mousePosition;

                Ray ray = Camera.main.ScreenPointToRay(position);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                        GameObject tappedGameObject = hit.collider.gameObject;
				    	//tapped object was a tree
                        if (tappedGameObject != null && tappedGameObject.GetComponentInParent<TreeScript>())
                        {
                            currentTreeDisplayingPopup = tappedGameObject.GetComponentInParent<TreeScript>();
                            currentTreeDisplayingPopup.ShowTreePopup();
                        }
                    }
                }
            }
            //drag
            else
            {
                //do nothing
                return;
            }
        }
    }

}
