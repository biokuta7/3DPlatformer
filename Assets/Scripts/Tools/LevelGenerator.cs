using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	public Texture2D map;
	public float offsetMultiplier = 2.0f;
	public Vector3 offset = Vector3.zero;

	[System.Serializable]
	public class TileData {
		public Color color;
		public GameObject prefab;
	}

	public List<TileData> tileData;

	public void GenerateLevel() {

		DestroyLevel (false);

		foreach (TileData t in tileData) {
			GameObject empty = new GameObject (t.prefab.name.ToUpper () + "S");
			empty.transform.localPosition = Vector3.zero;
			empty.transform.localRotation = Quaternion.identity;
			empty.transform.SetParent (transform);
			empty.isStatic = t.prefab.isStatic;

		}

		for (int x = 0; x < map.width; x++) {
			for (int y = 0; y < map.height; y++) {
				GenerateTile (x, y);
			}
		}

		DestroyLevel (true);
	}

	public void DestroyLevel(bool onlyClean) {

		GameObject[] children = new GameObject[transform.childCount];

		for (int i = 0; i < transform.childCount; i++) {
			GameObject g = transform.GetChild (i).gameObject;
			children [i] = g;
		}

		foreach (GameObject child in children) {
			if (!onlyClean || onlyClean && child.transform.childCount <= 0) {
				GameObject.DestroyImmediate (child);
			}

		}
	}

	void GenerateTile(int x, int y) {
		Color pixelColor = map.GetPixel (x, y);

		if (pixelColor.a == 0) {
			return;
		}

		int i = 0;

		foreach(TileData t in tileData) {

			if (pixelColor.Equals (t.color)) {
				Vector3 position = offset + t.prefab.transform.position + new Vector3 (x * offsetMultiplier, 0.0f, y * offsetMultiplier);
				GameObject g = Instantiate (t.prefab, position, Quaternion.identity);
				g.name = t.prefab.name.ToLower () + " ( " + x + " , " + y + " )";
				g.transform.SetParent (transform.GetChild(i));
			}

			i++;

		}

	}
}
