using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class OnMovingPlatform : MonoBehaviour {

	public LayerMask groundMask;
	private int groundMaskValue;

	void Start() {
		groundMaskValue = groundMask.value;
	}

	void LateUpdate() {
		CheckGround ();
	}

	void CheckGround() {

		Ray ray = new Ray (transform.position + transform.up * 1.0f, transform.up * -1.0f);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 5.0f, groundMaskValue)) {

			ObjectPath path = hit.collider.GetComponent<ObjectPath> ();
			if (path != null) {

				Move (path);

			}

		}

	}


	void Move(ObjectPath path) {
		transform.position += (path.GetDeltaPos ());
	}
}
