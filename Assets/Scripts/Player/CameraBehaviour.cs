using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	public Transform target;
	public float speed = 5.0f;

	private Vector3 offset = new Vector3(-5, 10, -5);

	public void Update() {

		Vector3 targetPos = new Vector3 (target.position.x + offset.x, offset.y, target.position.z + offset.z);

		transform.position = Vector3.Lerp (transform.position, targetPos, Time.deltaTime * speed);

	}
}
