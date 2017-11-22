using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPath : MonoBehaviour {

	public bool drawGizmos = true;
	public bool goBackToFirst = false;

	public enum SwingType {
		LOOP,
		ONCE
	}

	public SwingType swingType;

	public List<Vector3> nodes;

	public AnimationCurve curve;

	public float time;
	public float elapsedTime;
	public int index = 0;

	Vector3 startPos;
	Vector3 endPos;
	Vector3 deltaPos;
	Vector3 offset;

	private void Start() {

		offset = transform.position;

		if (goBackToFirst && swingType.Equals(SwingType.LOOP)) {
			nodes.Add (nodes [0]);
		}
	}

	public Vector3 GetDeltaPos() {
		return deltaPos;
	}

	public void SetCurve(AnimationCurve ac) {
		curve = ac;
	}

	void OnDrawGizmos() {

		if (drawGizmos && nodes.Count > 0) {
			Vector3 previousPos = (goBackToFirst? nodes[nodes.Count-1] : nodes [0]);

			foreach (Vector3 node in nodes) {
				Gizmos.color = Color.blue;
				Gizmos.DrawWireCube (node + offset, transform.lossyScale);
				Gizmos.color = Color.red;
				Gizmos.DrawLine (previousPos + offset, node + offset);
				Gizmos.DrawWireSphere (node + offset, .3f);

				for (int i = 0; i < 10; i++) {

					float tween = ((float)i) / 5.0f;

					Vector3 pos = Vector3.Lerp (previousPos + offset, node + offset, -1 + tween + (index%2 * (elapsedTime % 1.0f)));

					Vector3 forwards = (node - previousPos).normalized;

					Gizmos.DrawLine (pos, pos - .2f * (forwards + Vector3.up));
					Gizmos.DrawLine (pos, pos - .2f * (forwards - Vector3.up));

				}

				previousPos = node;
			}


		}

	}
		
	public void Update() {

		if (index > -1) {
			startPos = transform.position;
			elapsedTime += Time.deltaTime;

			Vector3 t1 = nodes [index/2];
			Vector3 t2 = nodes [Mathf.FloorToInt((index + 1)/2)];

			transform.position = Vector3.Lerp (t1, t2, curve.Evaluate(elapsedTime / time));

			endPos = transform.position;
			deltaPos = endPos - startPos;

			if (elapsedTime >= time) {
				elapsedTime = 0.0f;
				index++;
			}

			if (index > (nodes.Count*2) - (goBackToFirst? 3 : 2)) {
				index = (swingType.Equals (SwingType.LOOP) ? 0 : -1);
			}
				
		}
	}

}
