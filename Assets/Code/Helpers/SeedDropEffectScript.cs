using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedDropEffectScript : MonoBehaviour 
{

    [SerializeField]
    private GameObject animal;

    [SerializeField]
    private GameObject particlesPrefab;

    [SerializeField]
    private float effectDuration;

    private bool rotate;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update () 
    {
        //if (rotate)
        {
            Debug.Log("rotating");
            transform.RotateAround(animal.transform.position, Vector3.up, 360 * Time.deltaTime);    
        }
	}

    public IEnumerator CreateEffect()
    {
        Debug.Log("EFFECT");
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;

        GameObject particles = Instantiate(particlesPrefab, transform.position, transform.rotation, transform);

        rotate = true;
        yield return new WaitForSeconds(effectDuration);
        Destroy(particles);

        rotate = false;
    }
}
