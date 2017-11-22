using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[System.Serializable]
	public class Movement {
		public bool airControl = false;
		public float joystickPreciseMovementCutoff = .5f;
		public float moveSpeed = 2.5f;
		[Range(0.1f, 1.0f)]
		public float moveSmooth = .5f;

		public float gravityStrength = 9.81f;
		public float jumpStrength = 10.0f;

		public float yDeathThreshold = -100f;

		public Vector2 checkPoint = Vector2.zero;


		[HideInInspector]
		public bool isGrounded = false;
		[HideInInspector]
		public Vector3 flatMovementVector = Vector3.zero;
		[HideInInspector]
		public Vector3 vertMovementVector = Vector3.zero;
		[HideInInspector]
		public CharacterController controller;
		[HideInInspector]
		public OnMovingPlatform onMovingPlatform;

	}

	[System.Serializable]
	public class AnimationSettings {
		[HideInInspector]
		public Animator animator;
		[HideInInspector]
		public float speed;
	}

	private Camera cam;
	public Movement movement;
	public AnimationSettings animationSettings;
	void Start() {
		cam = Camera.main;
		movement.onMovingPlatform = GetComponent<OnMovingPlatform> ();
		movement.controller = GetComponent<CharacterController> ();
		animationSettings.animator = GetComponent<Animator> ();
		Die ();
	}

	void Update() {
		RayCheck ();
		Locomotion ();

		CheckDeath ();
	}

	void FixedUpdate() {
		LocomotionVertical ();
	}

	void RayCheck() {

		Ray forwards = new Ray (transform.position, transform.forward);
		Ray down = new Ray (transform.position, -transform.up);
		RaycastHit forwardsHit;
		RaycastHit downHit;

	}

	void OnTriggerEnter(Collider c) {

		Checkpoint checkpoint = c.GetComponent<Checkpoint> ();

		if (checkpoint != null && !checkpoint.GetFlag()) {
			movement.checkPoint = new Vector2 (c.transform.position.x, c.transform.position.z);
			checkpoint.SetFlag (true);
		}
			
	}

	public void Die() {

		transform.position = new Vector3 (movement.checkPoint.x, -movement.yDeathThreshold, movement.checkPoint.y);
		movement.flatMovementVector = Vector3.zero;

	}

	void CheckDeath() {

		if (transform.position.y <= movement.yDeathThreshold) {
			Die ();
		}

	}

	void LocomotionVertical() {
		//VERTICAL
		movement.isGrounded = movement.controller.isGrounded;
		movement.onMovingPlatform.SetGrounded (movement.isGrounded);
		animationSettings.animator.SetBool ("isGrounded", movement.isGrounded);

		if (movement.isGrounded) {
			movement.vertMovementVector = Vector3.zero;
			if(Input.GetButton("Jump")) {
				movement.vertMovementVector.y = movement.jumpStrength * .1f;
			}

		} else {

		}

		movement.vertMovementVector += new Vector3 (0.0f, -movement.gravityStrength * Time.deltaTime, 0.0f);
	}

	void Locomotion() {

		//FLAT

		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

		float speedMultiplier = Mathf.Clamp (input.sqrMagnitude, movement.joystickPreciseMovementCutoff, 1.0f);

		Vector3 cameraOrientationForward = new Vector3 (cam.transform.forward.x, 0.0f, cam.transform.forward.z).normalized;
		Vector3 cameraOrientationRight = new Vector3 (cam.transform.right.x, 0.0f, cam.transform.right.z).normalized;

		Vector3 targetFlatMovementVector = (movement.airControl || movement.isGrounded? 
			((input.x * cameraOrientationRight + input.z * cameraOrientationForward).normalized * movement.moveSpeed * speedMultiplier * Time.deltaTime) :
			movement.flatMovementVector
		);

		animationSettings.speed = Mathf.Lerp (animationSettings.speed, (input.sqrMagnitude>0? speedMultiplier : 0.0f), Time.deltaTime * 10.0f);

		animationSettings.animator.SetFloat ("Speed", Mathf.Clamp01(animationSettings.speed));

		if (targetFlatMovementVector.sqrMagnitude > 0.0001f) {
			Quaternion targetRotation = Quaternion.LookRotation (targetFlatMovementVector);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10.0f * Time.deltaTime);
		}

		movement.flatMovementVector = Vector3.Lerp (movement.flatMovementVector, targetFlatMovementVector, 1.0f - (movement.moveSmooth));



		//FINAL
		movement.controller.Move (movement.flatMovementVector + movement.vertMovementVector);



	}


}
