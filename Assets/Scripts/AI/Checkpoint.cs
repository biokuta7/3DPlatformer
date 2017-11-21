using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	Material mat;
	private bool flag = false;

	private static GameObject[] allPoints;

	public void Awake() {
		allPoints = GameObject.FindGameObjectsWithTag ("Checkpoint");
	}

	public void Start() {
		mat = GetComponent<Renderer> ().material;
	}

	public void SetFlag(bool f) {
		flag = f;
		if (flag) {
			StartCoroutine (ColorOn ());
			OtherFlagsOff ();
		} else {
			mat.color = Color.white;
		}
	}

	private void OtherFlagsOff() {

		foreach (GameObject point in allPoints) {

			if (!point.Equals (gameObject)) {

				point.GetComponent<Checkpoint> ().SetFlag (false);

			}

		}

	}

	public bool GetFlag() {
		return flag;
	}



	IEnumerator ColorOn() {

		for (int i = 0; i < 100; i++) {
			mat.color = Color.Lerp (Color.white, Color.red, ((float)i) / 100f);
			yield return new WaitForEndOfFrame();
		}

	}

}
