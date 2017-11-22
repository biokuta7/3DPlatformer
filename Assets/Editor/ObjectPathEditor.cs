using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ObjectPath))]
public class ObjectPathEditor : Editor {

	private ReorderableList list;
	private AnimationCurve ac = AnimationCurve.Linear(0, 0, 1, 1);

	private void OnEnable() {
		list = new ReorderableList (serializedObject,
			serializedObject.FindProperty ("nodes"),
			true, true, true, true);
	
		list.drawElementCallback = 
			(Rect rect, int index, bool isActive, bool isFocused) => {
			var element = list.serializedProperty.GetArrayElementAtIndex (index);
			rect.y += 2;
			EditorGUI.PropertyField (
				new Rect (rect.x, rect.y, rect.width - 30, EditorGUIUtility.singleLineHeight),
				element, new GUIContent("Node"));
	
		};
	}

	protected virtual void OnSceneGUI() {
		ObjectPath objectPath = (ObjectPath)target;

		Handles.color = Color.red;

		for(int i = 0; i < objectPath.nodes.Count; i++) {



			EditorGUI.BeginChangeCheck ();


			Vector3 targetPos = Handles.PositionHandle (objectPath.nodes [i], Quaternion.identity);


			if (EditorGUI.EndChangeCheck ()) {
				Undo.RecordObject (objectPath, "changed node position");
				objectPath.nodes [i] = targetPos;
			}
		}
	}

	public override void OnInspectorGUI() {

		EditorGUILayout.LabelField (
			"OBJECT PATH - " +
			"Use this for moving platforms and such.");

		serializedObject.Update ();

		ObjectPath objectPath = (ObjectPath)target;
		objectPath.swingType = (ObjectPath.SwingType) EditorGUILayout.EnumPopup ("Swing Type", objectPath.swingType);
		if(objectPath.swingType.Equals(ObjectPath.SwingType.LOOP)) {
			objectPath.goBackToFirst = EditorGUILayout.Toggle ("Loop back to first?", objectPath.goBackToFirst);
		}

		ac = EditorGUILayout.CurveField ("Movement Curve", ac);
		objectPath.SetCurve (ac);

		objectPath.time = EditorGUILayout.FloatField ("Time per Motion", objectPath.time);
		//objectPath.elapsedTime = EditorGUILayout.FloatField ("Starting Delay", objectPath.elapsedTime);
		EditorGUILayout.LabelField(
			"NODES - "+
			"Vector3's for positions.");

		list.DoLayoutList ();

		serializedObject.ApplyModifiedProperties ();



	}
	

}
