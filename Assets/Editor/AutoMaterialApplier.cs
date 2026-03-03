using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class AutoMaterialApplier : EditorWindow
{
    private GameObject modelPrefab;
    private DefaultAsset materialsFolder;

    private Dictionary<string, Material> materialLookup =
        new Dictionary<string, Material>();

    [MenuItem("Tools/Auto Apply Materials (Final Working Version)")]
    public static void ShowWindow()
    {
        GetWindow<AutoMaterialApplier>("Material Auto Matcher");
    }

    private void OnGUI()
    {
        modelPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Model Prefab", modelPrefab, typeof(GameObject), false);

        materialsFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Materials Folder", materialsFolder, typeof(DefaultAsset), false);

        if (GUILayout.Button("Apply Materials"))
        {
            if (modelPrefab == null || materialsFolder == null)
            {
                Debug.LogError("Assign both fields.");
                return;
            }

            ApplyMaterials();
        }
    }

    private void ApplyMaterials()
    {
        materialLookup.Clear();

        string folderPath = AssetDatabase.GetAssetPath(materialsFolder);
        string[] guids = AssetDatabase.FindAssets("t:Material", new[] { folderPath });

        // Build dictionary from materials
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            string id = ExtractMaterialID(mat.name);

            if (!string.IsNullOrEmpty(id) && !materialLookup.ContainsKey(id))
            {
                materialLookup.Add(id, mat);
            }
        }

        string prefabPath = AssetDatabase.GetAssetPath(modelPrefab);
        GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);

        Renderer[] renderers = prefabRoot.GetComponentsInChildren<Renderer>(true);

        int appliedCount = 0;

        foreach (Renderer renderer in renderers)
        {
            string objectID = ExtractObjectID(renderer.gameObject.name);

            if (materialLookup.TryGetValue(objectID, out Material matchedMaterial))
            {
                renderer.sharedMaterial = matchedMaterial;
                appliedCount++;
            }
        }

        PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
        PrefabUtility.UnloadPrefabContents(prefabRoot);

        Debug.Log($"✅ Applied {appliedCount} materials successfully.");
    }

    private string ExtractObjectID(string name)
    {
        Match match = Regex.Match(name, @"\.Shape_([^_]+)");
        if (match.Success)
            return match.Groups[1].Value.ToLower();

        return "";
    }

    private string ExtractMaterialID(string name)
    {
        // Extract part between last "_" and "_albedo" or "_albed"
        Match match = Regex.Match(name, @"_([^_]+)_albedo?$", RegexOptions.IgnoreCase);
        if (match.Success)
            return match.Groups[1].Value.ToLower();

        return "";
    }
}