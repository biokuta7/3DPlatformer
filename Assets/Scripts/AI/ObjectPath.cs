using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPath : MonoBehaviour {

	public bool drawGizmos = true;

	public enum SwingType {
		LOOP,
		ONCE
	}

	public SwingType swingType;

	public Transform[] nodes;
	public AudioClip[] clips;
	private AudioSource audioSource;


	public float time;
	public float elapsedTime;
	public int index = 0;

	Vector3 startPos;
	Vector3 endPos;
	Vector3 deltaPos;

	public Vector3 GetDeltaPos() {
		return deltaPos;
	}

	void OnDrawGizmos() {
		if (drawGizmos) {

			Gizmos.color = Color.blue;

			foreach (Transform node in nodes) {
				Gizmos.DrawWireCube (node.position, transform.lossyScale);
			}


		}
	}
		
	public void Update() {

		startPos = transform.position;
		elapsedTime += Time.deltaTime;

		if (elapsedTime >= time) {
			elapsedTime = 0.0f;
			index++;
		}

		if (index > nodes.Length - 2) {
			index = 0;
		}

		Transform t1 = nodes [index];
		Transform t2 = nodes [index + 1];


		transform.position = Vector3.Lerp (t1.position, t2.position, elapsedTime / time);

		endPos = transform.position;
		deltaPos = endPos - startPos;

	}

	public void UpdateAudio() {

	}

}
