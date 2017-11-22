using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TimeOfDay : MonoBehaviour {

	public Light directionalLight;
	public Color dayColor = Color.blue;
	public Color nightColor = Color.black;
	public Color dayLightColor = Color.yellow;
	public Color nightLightColor = Color.blue;
	public float cycleTime = 100.0f;
	public bool day = true;

	private float elapsedTime = 0.0f;
	private Camera cam;

	void Start() {
		cam = Camera.main;
	}

	void Update() {

		elapsedTime += Time.deltaTime;

		if (elapsedTime >= cycleTime) {
			elapsedTime = 0.0f;
			day = !day;
		}

		float elapsedPercent = elapsedTime / cycleTime;

		Color camColorLerpStart = (day? dayColor : nightColor);
		Color camColorLerpEnd =  (!day? dayColor : nightColor);

		Color camColor = Color.Lerp (camColorLerpStart, camColorLerpEnd, elapsedPercent);

		Color lightColorLerpStart = (day? dayLightColor : nightLightColor);
		Color lightColorLerpEnd = (!day? dayLightColor : nightLightColor);

		Color lightColor = Color.Lerp (lightColorLerpStart, lightColorLerpEnd, elapsedPercent);

		cam.backgroundColor = camColor;
		directionalLight.color = lightColor;

	}


}
