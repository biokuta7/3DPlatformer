using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class OnMovingPlatform : MonoBehaviour {

	public LayerMask groundMask;
	private int groundMaskValue;
	private bool grounded = false;

	void Start() {
		groundMaskValue = groundMask.value;
	}

	public void SetGrounded(bool g) {
		grounded = g;
	}

	void LateUpdate() {
		if (grounded) {
			CheckGround ();
		}
	}

	void CheckGround() {

		Ray ray = new Ray (transform.position + transform.up * 1.0f, transform.up * -1.0f);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 10.0f, groundMaskValue)) {

			ObjectPath path = hit.collider.GetComponent<ObjectPath> ();
			if (path != null && (hit.distance < 2.5f || grounded)) {

				Move (path);

			}

		}

	}


	void Move(ObjectPath path) {
		transform.position += (path.GetDeltaPos ());
	}
}
