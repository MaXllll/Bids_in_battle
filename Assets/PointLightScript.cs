using UnityEngine;
using System.Collections;

public class PointLightScript : MonoBehaviour {

	private Light light;


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
		
		if(light.intensity < 2.0f)
		{
			intensityDown = false;
		} if (light.intensity > 8.0f){
			intensityDown = true;
		}
	}
}
