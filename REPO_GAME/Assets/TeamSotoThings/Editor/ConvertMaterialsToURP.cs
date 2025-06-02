using UnityEngine;
using UnityEditor;

public class ConvertMaterialsToURP
{
    [MenuItem("Tools/Convert Materials to URP")]
    static void ConvertAllMaterials()
    {
        string[] materialGUIDs = AssetDatabase.FindAssets("t:Material");
        int count = 0;

        foreach (string guid in materialGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat.shader.name == "Standard")
            {
                mat.shader = Shader.Find("Universal Render Pipeline/Lit");
                Debug.Log("Converted: " + path);
                count++;
            }
        }

        Debug.Log($"Converted {count} materials to URP.");
    }
}
