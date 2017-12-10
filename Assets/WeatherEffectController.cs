using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Code.Controllers;
using UnityEngine;

public class WeatherEffectController : MonoBehaviour {

	[SerializeField]
	private GameObject rainParticlesPrefab;

	public bool IsRaining { get; set; }
	private TreeCollectionScript treeCollection;
	private int rainPerSecond;
	private float rainSeconds;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if (!IsRaining && ShouldRain())
		//{
			//StartCoroutine(Rain());
		//}
	}

	//holds the camera
	//adds effect to camera or not


	public IEnumerator Rain()
	{
		rainSeconds = Random.Range(10, 60);

		//switch the camera prefab to rain
		IsRaining = true;

		yield return new WaitForSeconds(rainSeconds);

		IsRaining = false;
	}

	public IEnumerator Storm()
	{
		yield return null;
	}

	public IEnumerator Fog()
	{
		yield return null;
	}
}
