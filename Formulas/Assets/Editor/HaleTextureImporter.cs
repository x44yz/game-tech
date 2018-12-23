using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// TODO:
// use TextureImporter
// https://sourceforge.net/p/hale/wiki/Home/
public class HaleTextureImporter
{
	[Serializable]
	private class HaleTextureJsonFormat
	{
		public string source = string.Empty;
		public int gridSize = 32;
		public Dictionary<string, List<int>> images = new Dictionary<string, List<int>>();
	}

	[MenuItem("Tools/Import Hale Texture")]
	public static void Import()
	{
		var objs = GetSelectedTextures();
		for (int i = 0; i < objs.Length; ++i)
		{
			string texPath = AssetDatabase.GetAssetPath(objs[i]);
			string jsonPath = texPath.Substring(0, texPath.LastIndexOf('.')) + ".json";
			TextAsset jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
			if (jsonAsset == null)
			{
				Debug.LogWarning("failed to find json file: " + jsonPath);
				continue;
			}
			HaleTextureJsonFormat jsonInfo = JsonUtility.FromJson<HaleTextureJsonFormat>(jsonAsset.ToString());
			Debug.Log("gridSize: " + jsonInfo.gridSize);

			Debug.Log("import texture file: " + texPath);

			TextureImporter imp = AssetImporter.GetAtPath(texPath) as TextureImporter;
			imp.textureType = TextureImporterType.Sprite;
		}
	}

	private static UnityEngine.Object[] GetSelectedTextures()
	{
		return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
	}
}
