using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// using SimpleJson;

// TODO:
// use TextureImporter
// https://sourceforge.net/p/hale/wiki/Home/
public class HaleTextureImporter
{
	private const SpriteAlignment SPR_ALIGNMENT = SpriteAlignment.BottomCenter;

	[MenuItem("Tools/Import Hale Texture")]
	public static void Import()
	{
		var objs = GetSelectedTextures();
		for (int i = 0; i < objs.Length; ++i)
		{
			Texture2D tex = (Texture2D)objs[i];
			int texHeight = tex.height;

			string texPath = AssetDatabase.GetAssetPath(objs[i]);
			string jsonPath = texPath.Substring(0, texPath.LastIndexOf('.')) + ".json";
			TextAsset jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
			if (jsonAsset == null)
			{
				Debug.LogWarning("failed to find json file: " + jsonPath);
				continue;
			}

			List<SpriteMetaData> sps = new List<SpriteMetaData>();

			int offGridX = 0, offGridY = 0;
			if (SPR_ALIGNMENT == SpriteAlignment.BottomCenter)
			{
				offGridX = 0;
				offGridY = -1;
			}
			else
				throw new System.NotImplementedException();

			var jsonInfo = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(jsonAsset.text);
			string source = (string)jsonInfo["source"];
			int gridSize = Convert.ToInt32(jsonInfo["gridSize"]);
			var images = (IDictionary<string, object>)jsonInfo["images"];
			foreach (var objImage in images)
			{
				string imgName = (string)objImage.Key;
				var gp = (SimpleJson.JsonArray)objImage.Value;
				int gx = Convert.ToInt32(gp[0]);
				int gy = Convert.ToInt32(gp[1]);

				SpriteMetaData sp = new SpriteMetaData();
				sp.name = imgName;
				sp.alignment = (int)SPR_ALIGNMENT;

				sp.rect = new Rect(gx * gridSize + offGridX * gridSize, 
													texHeight - gy * gridSize + offGridY * gridSize, 
													gridSize, gridSize);
				sps.Add(sp);
			}

			// Debug.Log("start import texture file: " + texPath);

			TextureImporter imp = AssetImporter.GetAtPath(texPath) as TextureImporter;
			imp.textureType = TextureImporterType.Sprite;
			imp.spritesheet = sps.ToArray();
			
			AssetDatabase.ImportAsset(texPath);
			Debug.Log("finish import texture file: " + texPath);
		}
	}

	private static UnityEngine.Object[] GetSelectedTextures()
	{
		return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
	}
}
