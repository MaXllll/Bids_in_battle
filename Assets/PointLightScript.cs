using UnityEngine;
using System.Collections;

public class PointLightScript : MonoBehaviour {

	private Light light;

	bool intensityUp = false;

	bool intensityDown = true;
	// Use this for initialization
	void Start () {
		light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {

		if(intensityDown)
		{
			light.intensity -= 1.0f;
		} else {
			light.intensity += 1.0f;
		}
		 
	}
}
