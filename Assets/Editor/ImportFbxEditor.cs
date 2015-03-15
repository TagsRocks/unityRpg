using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(ImportFbx))]
public class ImportFbxEditor : Editor {
	string fbxDir = "";
	public override void OnInspectorGUI ()
	{
		fbxDir = GUILayout.TextField(fbxDir);
		if(GUILayout.Button("Import Fbx"))  {
			var allModel = Path.Combine(Application.dataPath,  fbxDir);
			var resDir = new DirectoryInfo(allModel);
			FileInfo[] fileInfo = resDir.GetFiles("*.fbx", SearchOption.AllDirectories);
			AssetDatabase.StartAssetEditing();

			foreach(FileInfo file in fileInfo) {
				var ass = Path.Combine("Assets/"+fbxDir, file.Name);
				var import = ModelImporter.GetAtPath(ass) as ModelImporter;
				import.globalScale = 1;
				import.importAnimation = true;
				import.animationType = ModelImporterAnimationType.Legacy;
				//var ani = import.clipAnimations[0];

				AssetDatabase.WriteImportSettingsIfDirty(ass);
			}


			fileInfo = resDir.GetFiles("*.tga", SearchOption.AllDirectories);
			foreach(FileInfo file in fileInfo) {
				Debug.Log("Find tga "+file.Name);
				Debug.Log("tga name "+file.FullName);
				var realPath = file.FullName.Replace(Application.dataPath, "");
				Debug.Log("real "+realPath);
				var assPath = "Assets"+realPath;
				var import = TextureImporter.GetAtPath(assPath) as TextureImporter;
				import.mipmapEnabled = false;
				AssetDatabase.WriteImportSettingsIfDirty(assPath);
			}
			AssetDatabase.StopAssetEditing();
			AssetDatabase.Refresh();
		}
		if (GUILayout.Button ("Adjust Material ")) {
			var allModel = Path.Combine(Application.dataPath,  fbxDir);
			var resDir = new DirectoryInfo(allModel);
			FileInfo[] fileInfo = resDir.GetFiles("*.prefab", SearchOption.AllDirectories);
			foreach(FileInfo file in fileInfo) {
				var assPath = "Assets"+file.FullName.Replace(Application.dataPath, "");
				var prefab = Resources.LoadAssetAtPath<GameObject>(assPath);
				Debug.Log("load prefab "+assPath);
				SetRender(prefab.transform);
				foreach(Transform t in prefab.transform) {
					SetRender(t);
				}

			}
		}
	}
	static void SetRender(Transform prefab) {
		if (prefab.renderer != null) {
			prefab.renderer.sharedMaterial.shader = Shader.Find ("Custom/playerShader");
			prefab.renderer.sharedMaterial.color = Color.white;
			prefab.renderer.sharedMaterial.SetColor ("_Ambient", Color.white);
			EditorUtility.SetDirty (prefab.renderer.sharedMaterial);
		}
	}
}
