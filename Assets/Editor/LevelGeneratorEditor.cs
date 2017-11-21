using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor {

	Texture2D map;
	private ReorderableList list;

	private void OnEnable() {
		list = new ReorderableList (serializedObject,
			serializedObject.FindProperty ("tileData"),
			true, true, true, true);

		list.drawElementCallback = 
			(Rect rect, int index, bool isActive, bool isFocused) => {
			var element = list.serializedProperty.GetArrayElementAtIndex (index);
			rect.y += 2;
			EditorGUI.PropertyField (
				new Rect (rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative ("color"), GUIContent.none);
			EditorGUI.PropertyField (
				new Rect (rect.x + 60, rect.y, rect.width - 90, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative ("prefab"), GUIContent.none);
		};
	}

	public override void OnInspectorGUI() {
		
		serializedObject.Update ();

		LevelGenerator generator = (LevelGenerator)target;

		GUILayout.Label ("Generates a level based on given map data.");
		generator.offset = EditorGUILayout.Vector3Field ("Map Offset", generator.offset);
		generator.offsetMultiplier = EditorGUILayout.FloatField ("Tile Size", generator.offsetMultiplier);

		map = EditorGUILayout.ObjectField ("Map Texture", map, typeof(Texture2D), true) as Texture2D;

		list.DoLayoutList ();


		if (GUILayout.Button ("Load Level")) {
			generator.GenerateLevel ();
		}

		if (GUILayout.Button ("Destroy Level")) {
			generator.DestroyLevel (false);
		}

		serializedObject.ApplyModifiedProperties ();

	}

}
